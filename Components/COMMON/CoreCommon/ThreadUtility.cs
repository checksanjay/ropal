using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    public static class ThreadUtility
    {
        public static void StandardRetryLogic(Action method, ReaderWriterLockSlim rwlock)
        {
            rwlock.EnterWriteLock();
            try
            {
                method();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                rwlock.ExitWriteLock();
            }           
        }
    }
}
