using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    public static class CommonUtilities
    {
        public static bool IsObjectEmpty(MailAddressCollection obj)
        {
            if (obj != null && obj.Count() > 0) return false;
            return true;
        }

        public static bool IsStringEmpty(string data)
        {
            if (String.IsNullOrEmpty(data)) return true;
            return false;
        }

        public static bool IsBytesEmpty(Byte[] bytes)
        {
            if (bytes != null || bytes.Length > 0) return false;
            return true;
        }

        public static bool IsObjectEmpty(Object obj)
        {
            if (obj != null ) return false;
            return true;
        }

        public static bool IsObjectEmpty(NameValueCollection nvc)
        {
            if (nvc != null && nvc.Count > 0) return false;
            return true;
        }     
    }
}
