using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    public static class TextSniffer
    {
        private static bool IsCharacter(string data)
        {
            if (String.IsNullOrEmpty(data))
                return false;

            return Regex.IsMatch(data, "[a-zA-Z]+");
        }

        private static bool IsNumber(string data)
        {
            if (String.IsNullOrEmpty(data))
                return false;

            return Regex.IsMatch(data, "[0-9]+");
        }

        public static bool SniffString( string data)
        {
            if (String.IsNullOrEmpty(data))
                return false;         

            string s_safeChars = GetDataFromWebConfig.SafeCharacters;
            foreach (char ch in data)
            {
                if (!Char.IsLetter(ch) &&
                    !Char.IsNumber(ch) &&
                    !s_safeChars.Contains(ch)
                    )
                {                    
                    return false;
                }

            }
            return true;
        }
    }
}
