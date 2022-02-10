using Ropal.CoreCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ropal.CoreCache
{
    public sealed class RopalSubCache<T> : IDisposable
    {
        private bool b_disposed;
        private ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
        public Dictionary<string, T> dict_Lookup = new Dictionary<string, T>();

        ~RopalSubCache()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        public void Dispose(bool disposing)
        {
            if (!b_disposed)
            {
                b_disposed = true;
                if (disposing)
                {
                    try
                    {
                        rwlock.Dispose();
                    }
                    catch { }
                }
            }
        }

        private void Get(RopalCacheKey key, out T template_obj)
        {
            template_obj = dict_Lookup[key.ToString()];
        }

        public T GetCache(RopalCacheKey key)
        {
            T template_obj = default(T);            
            ThreadUtility.StandardRetryLogic(delegate() { Get(key, out template_obj); }, rwlock);
            return template_obj;
        }

        private void Add(RopalCacheKey key, T template_obj)
        {
            string s_cacheKey = key.ToString();
            if (dict_Lookup.ContainsKey(s_cacheKey))
            {
                dict_Lookup[s_cacheKey] = template_obj;
            }
            else
            {
                dict_Lookup.Add(s_cacheKey, template_obj);
            }
        }

        public void AddCache(string key, T template_obj)
        {
            RopalCacheKey cachekey = new RopalCacheKey(key);
            ThreadUtility.StandardRetryLogic(delegate() { Add(cachekey, template_obj); }, rwlock);
        }

        private void ClearAll()
        {   
            dict_Lookup.Clear();
        }

        public void ClearAllCache()
        {
            ThreadUtility.StandardRetryLogic(delegate() { ClearAll(); }, rwlock);
        }

        private void Delete(RopalCacheKey key)
        {
            string s_cacheKey = key.ToString();
            if (dict_Lookup.ContainsKey(s_cacheKey))
            {
                dict_Lookup.Remove(s_cacheKey);
            }
        }

        public void DeleteCache(RopalCacheKey key, T template_obj)
        {
            ThreadUtility.StandardRetryLogic(delegate() { Delete(key); }, rwlock);
        }
    }
}
