using Ropal.CoreCommon;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ropal.CoreModule
{
    public class MainEntryModule : IHttpModule
    {
        public MainEntryModule()
        {
        }

        public String ModuleName
        {
            get { return "Ropal Main Entry Module"; }
        }


        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
                (new EventHandler(this.Application_BeginRequest));
            application.EndRequest +=
                (new EventHandler(this.Application_EndRequest));
            ProcessRequest();
        }

        private void Application_BeginRequest(Object source,
             EventArgs e)
        {            
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath;
            string fileExtension =
                VirtualPathUtility.GetExtension(filePath);
            //if (fileExtension.Equals(".aspx"))
            //{
            //    context.Response.Write("<h1><font color=red>" +
            //        "Ropal Main Entry Module: Beginning of Request" +
            //        "</font></h1><hr>");
            //}
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            string filePath = context.Request.FilePath;
            string fileExtension =
                VirtualPathUtility.GetExtension(filePath);
            //if (fileExtension.Equals(".aspx"))
            //{
            //    context.Response.Write("<hr><h1><font color=red>" +
            //        "Ropal Main Entry Module: End of Request</font></h1>");
            //}
        }

        public void Dispose() { }

        public void ProcessRequest()
        {
            NameValueCollection nvc = HttpLayerUtility.GetQSCollecton();
        }
    }
}
