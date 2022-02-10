using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ropal.CoreCommon
{
    public static class EncodeDecodeUtility
    {
        public static string HtmlEncode(string data)
        {
            if (CommonUtilities.IsStringEmpty(data)) return String.Empty;
            return HttpUtility.HtmlEncode(data);
        }

        public static string HtmlDecode(string data)
        {
            if (CommonUtilities.IsStringEmpty(data)) return String.Empty;
            return HttpUtility.HtmlDecode(data);
        }

        public static string UrlEncode(string data)
        {
            if (CommonUtilities.IsStringEmpty(data)) return String.Empty;
            return HttpUtility.UrlEncode(data);
        }

        public static string UrlDecodee(string data)
        {
            if (CommonUtilities.IsStringEmpty(data)) return String.Empty;
            return HttpUtility.UrlDecode(data);
        }

    }
}
