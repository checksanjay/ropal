using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreLog
{
    public static class Log4netUtility
    {
        private static readonly ILog l_Log = LogManager.GetLogger("Ropal");

        public static void Info(string  message)
        {   
            if (l_Log.IsInfoEnabled)
            {
                l_Log.Info("Info : " + message);
            }

        }

        public static void Error(string message)
        {           
            if (l_Log.IsErrorEnabled)
            {
                l_Log.Error("Error : " + message );
            }
        }
    
    }
}
