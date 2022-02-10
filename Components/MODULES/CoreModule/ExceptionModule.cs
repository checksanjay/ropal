using System;
using System.Web;

namespace Ropal.CoreModule
{
    public class ExceptionModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        
        public void Dispose()
        {            
           // Dispose();
        }

        public void Init(HttpApplication context)
        {         
            context.Error += new EventHandler(Application_Error);
        }
        
        public void Application_Error(object sender, EventArgs e)
        {
            Exception ex = HttpContext.Current.Server.GetLastError().GetBaseException();         
        }
    }
}
