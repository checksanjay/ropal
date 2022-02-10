using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCache
{
    public class RopalCacheKey
    {        
        private string FileName;
        
        public RopalCacheKey(string s_fileName)
        {
            FileName = s_fileName;
        }
    
        #region GetHashCode
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FileName);         
            return sb.ToString();
        }
        
    }
}