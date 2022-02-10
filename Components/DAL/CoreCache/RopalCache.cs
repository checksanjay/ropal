using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ropal;
using Ropal.CoreXmlWrapper;

namespace Ropal.CoreCache
{
    public static class RopalCache
    {
        public static LoadCss GetCacheObject_LoadCss(string key)
        {            
            RopalCacheKey o_RopalCacheKey = new RopalCacheKey(key);
            LoadCss o_LoadCss = RopalCache.LoadCssCache.GetCache(o_RopalCacheKey);
            if (IsObjectEmpty(o_LoadCss)) return null;
            return o_LoadCss;
        }

        public static bool IsObjectEmpty(LoadCss obj_LoadCss)
        {
            if (obj_LoadCss != null && obj_LoadCss.pageNames.Count() > 0) return false;
            return true;
        }

        public static RopalSubCache<LoadCss> LoadCssCache = new RopalSubCache<LoadCss>();

        
    }
}
