using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreExceptions
{
    public static class ExceptionMessages
    {
        public static string FileNotFound(string s_filePath)
        {
            return "File Not Found : " + s_filePath;            
        }
    }
}
