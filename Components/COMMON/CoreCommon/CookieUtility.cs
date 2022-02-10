﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ropal.CoreCommon
{
    public static class CookieUtility
    {
        public enum Type
        {
            SessionCookie,
            PersistenceCookie
        };

        public static string GetCookieValue(string cookieName)
        {
            string cookieVal = String.Empty;
            cookieVal = HttpContext.Current.Request.Cookies[cookieName].Value;
            return cookieVal;
        }

        public static string GetCookieValueFromResponse(string cookieName)
        {
            string cookieVal = String.Empty;
            cookieVal = HttpContext.Current.Response.Cookies[cookieName].Value;
            return cookieVal;
        }
        public static string GetNestedCookie(string cookieName, string cookieKey)
        {
            string cookieVal = String.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null) return String.Empty;
            
            return cookie[cookieKey];
        }

        public static void SetCookie(string cookieName, string value, int? expirationDays)
        {
            HttpCookie Cookie = new HttpCookie(cookieName, value);
            if (expirationDays.HasValue)
                Cookie.Expires = DateTime.Now.AddDays(expirationDays.Value);
            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        //public static void SetNestedCookie(string cookieName, Dictionary<string,string> list, int? expirationDays)
        //{
        //    HttpCookie Cookie = new HttpCookie(cookieName, value);
        //    if (expirationDays.HasValue)
        //        Cookie.Expires = DateTime.Now.AddDays(expirationDays.Value);
        //    HttpContext.Current.Response.Cookies.Add(Cookie);
        //}

        public static void DeleteCookie(string cookieName)
        {
            HttpCookie Cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (Cookie != null)
            {
                Cookie.Expires = DateTime.Now.AddDays(-2);
                HttpContext.Current.Response.Cookies.Add(Cookie);
            }
        }

        public static bool CookieExists(string cookieName)
        {
            bool exists = false;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
                exists = true;
            return exists;
        }

        public static Dictionary<string, string> GetAllCookies()
        {
            Dictionary<string, string> cookies = new Dictionary<string, string>();
            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                cookies.Add(key, HttpContext.Current.Request.Cookies[key].Value);
            }
            return cookies;
        }

        public static void DeleteAllCookies()
        {
            var x = HttpContext.Current.Request.Cookies;
            foreach (HttpCookie cook in x)
            {
                DeleteCookie(cook.Name);
            }
        }      
    }
}
