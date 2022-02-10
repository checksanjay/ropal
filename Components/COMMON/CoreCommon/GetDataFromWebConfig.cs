using Ropal;
using Ropal.CoreConstants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    public static class GetDataFromWebConfig
    {        
        public static string JQueryFileLocation
        {
            get
            {
                try
                {
                    string s_data = ConfigurationManager.AppSettings[WebConfigConstants.JQueryFileLocation];
                    if (!String.IsNullOrEmpty(s_data))
                    {
                        return "<script src=\"" + s_data + "\"type=\"text/javascript\"></script>";                        
                    }
                }
                catch                
                { 
                    // do logging
                }

                return "<script src=\"/Scripts/jquery/jquery-1.7.1.min.js\" type=\"text/javascript\"></script>";                
            }
        }

        public static string SafeCharacters
        {
            get
            {
                try
                {
                    string s_data = ConfigurationManager.AppSettings[WebConfigConstants.SafeCharacters];
                    if (!String.IsNullOrEmpty(s_data)) return s_data;                   
                }
                catch
                {
                    // do logging
                }
                return String.Empty;                
            }
        }
    }
}
