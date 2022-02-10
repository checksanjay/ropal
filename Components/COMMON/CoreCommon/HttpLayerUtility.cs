using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ropal.CoreCommon
{
    public static class HttpLayerUtility
    {
        public static string GetSpecificQS(string queryString)
        {
            NameValueCollection nvc = GetQSCollecton();
            if (CommonUtilities.IsObjectEmpty(nvc)) return String.Empty;
            return nvc[queryString];
        }

        public static NameValueCollection GetQSCollecton()
        {
            String currurl = HttpContext.Current.Request.RawUrl;
            if (CommonUtilities.IsStringEmpty(currurl)) return null;

            int iqs = currurl.IndexOf('?');
            if (iqs == -1) return null;

            String querystring = (iqs < currurl.Length - 1) ? currurl.Substring(iqs + 1) : String.Empty;
            NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);
            return qscoll;
        }
    }
}
