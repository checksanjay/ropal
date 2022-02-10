using System;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

namespace Ropal.CoreCommon
{
    public class CssHandler: IHttpHandler
    {
        #region IHttpHandler implementation

        public void ProcessRequest(HttpContext context)
        {
            FileInfo file = new FileInfo(context.Request.PhysicalPath);
            if (file.Extension.Equals(".css", StringComparison.OrdinalIgnoreCase))
            {
                SetDefaultVariables();
                ParseVariables(file.FullName);
                ApplyVariables(context);
                ReduceSize(context);
                SetHeadersAndCache(file.FullName, context);
                _Variables.Clear();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Private members

        private Dictionary<string, string> _Variables = new Dictionary<string, string>();
        private StringBuilder _CleanedCSS = new StringBuilder();
        private StringBuilder _ParsedCSS = new StringBuilder();

        #endregion

        #region Methods

        /// <summary>
        /// Adds the built-in variables to the collection.
        /// </summary>
        private void SetDefaultVariables()
        {
            _Variables.Add("browser", "\"" + HttpContext.Current.Request.Browser.Browser + "\"");
            _Variables.Add("version", HttpContext.Current.Request.Browser.MajorVersion.ToString());
        }

        /// <summary>
        /// Parses the variables defined in the stylesheet
        /// and adds them to the variable collection.
        /// </summary>
        private void ParseVariables(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith("define "))
                    {
                        line = line.Replace("define ", string.Empty);
                        int index = line.IndexOf("=") + 1;
                        string key = line.Substring(0, index - 1).Trim();
                        string value = line.Substring(index, line.Length - index).Replace(";", string.Empty).Trim();

                        foreach (string var in _Variables.Keys)
                        {
                            if (value.Contains(var))
                                value = value.Replace(var, _Variables[var]);
                        }

                        value = ProcessExpression(value);

                        _Variables.Add(key, value);
                    }
                    else
                    {
                        _CleanedCSS.AppendLine(line);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the defined variables to the stylesheet.
        /// </summary>
        private void ApplyVariables(HttpContext context)
        {
            string css = _CleanedCSS.ToString();
            foreach (string variable in _Variables.Keys)
            {
                css = css.Replace(variable, _Variables[variable]);
            }

            _ParsedCSS.Append(css);
        }

        /// <summary>
        /// A simple function to get the result of a C# expression
        /// </summary>
        /// <param name="command">String value containing an expression that can evaluate to a string.</param>
        /// <returns>A string value after evaluating the command string.</returns>
        private string ProcessExpression(string expression)
        {
            if (expression.Contains("System."))
                throw new ArgumentException("Command is not allowed: " + expression);

            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;

                string source = "namespace dotnetslave{" +
                                "class css{" +
                                "public static object Evaluate(){return " + expression + ";}}} ";

                CompilerResults result = provider.CompileAssemblyFromSource(parameters, source);

                if (result.Errors.Count > 0)
                {
                    return expression;
                }
                else
                {
                    MethodInfo Methinfo = result.CompiledAssembly.GetType("dotnetslave.css").GetMethod("Evaluate");
                    return Methinfo.Invoke(null, null).ToString();
                }
            }
        }

        /// <summary>
        /// Removes all unwanted text from the CSS file,
        /// including comments and whitespace.
        /// </summary>
        private void ReduceSize(HttpContext context)
        {
            string css = _ParsedCSS.ToString();
            css = css.Replace("  ", String.Empty);
            css = css.Replace(Environment.NewLine, String.Empty);
            css = css.Replace("\t", string.Empty);
            css = css.Replace(" {", "{");
            css = css.Replace(" :", ":");
            css = css.Replace(": ", ":");
            css = css.Replace(", ", ",");
            css = css.Replace("; ", ";");
            css = css.Replace(";}", "}");
            css = Regex.Replace(css, @"/\*[^\*]*\*+([^/\*]*\*+)*/", "$1");
            css = Regex.Replace(css, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", String.Empty);

            context.Response.Write(css);
        }

        /// <summary>
        /// This will make the browser and server keep the output
        /// in its cache and thereby improve performance.
        /// </summary>
        private void SetHeadersAndCache(string file, HttpContext context)
        {
            context.Response.ContentType = "text/css";
            context.Response.AddFileDependency(file);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.VaryByParams["path"] = true;
            context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetLastModifiedFromFileDependencies();
        }

        #endregion

    }
}
