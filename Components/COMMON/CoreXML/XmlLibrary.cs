using Ropal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Ropal;
using Ropal.CoreExceptions;

namespace Ropal.CoreXML
{
    public static class XmlLibrary
    {
        public static void LoadXMLFile_GetRootNode(string fileName, string s_RootNode, out XDocument x_data, out XElement x_rootNode)
        {
            x_rootNode = null;
            x_data = null;
            if (String.IsNullOrEmpty(fileName)) return;
            IXPathNavigable l_Document = new XmlDocument();            
            if (LoadFile(l_Document, fileName))
            {
                string s_fileData = ((XmlDocument)l_Document).InnerXml;
                x_data = XDocument.Parse(s_fileData);
                x_rootNode = x_data.XPathSelectElement("//" + s_RootNode);
            }
        }

        public static bool LoadFile(IXPathNavigable l_Document, string fileName)
        {            
            string s_filePath = GetCompleteFilePath(fileName);
            if (String.IsNullOrEmpty(s_filePath)) throw new Exception();
            if (!File.Exists(s_filePath)) throw new FileNotFoundException(ExceptionMessages.FileNotFound(s_filePath));            
            ((XmlDocument)l_Document).Load(s_filePath);
            return true;

        }

        public static string GetCompleteFilePath(string s_fileName)
        {
            return GetReleaseFolderBasePath() + s_fileName;
        }

        public static string GetReleaseFolderBasePath()
        {
            string str = AppDomain.CurrentDomain.BaseDirectory;
            return str + "Ropal_Release\\Data\\";
        }

        public static IEnumerable<XElement> LoadFile_GetNodes(string filePath, string s_RootNode, string s_collectionNodeName, out XDocument x_data, out XElement x_rootNode)
        {
            LoadXMLFile_GetRootNode(filePath, s_RootNode, out x_data, out x_rootNode);
            if (x_rootNode == null)
                return null;
            return x_rootNode.Elements(s_collectionNodeName);
        }

        public static string GetXPathFromXElement(XElement element)
        {
            return string.Join("/", element.AncestorsAndSelf().Reverse()
                .Select(e =>
                {
                    var index = GetIndex(e);

                    if (index == 1)
                    {
                        return e.Name.LocalName;
                    }

                    return string.Format("{0}[{1}]", e.Name.LocalName, GetIndex(e));
                }));

        }

        private static int GetIndex(XElement element)
        {
            var i = 1;
            if (element.Parent == null)
            {
                return 1;
            }
            foreach (var e in element.Parent.Elements(element.Name.LocalName))
            {
                if (e == element)
                {
                    break;
                }
                i++;
            }
            return i;
        }
    }

}
