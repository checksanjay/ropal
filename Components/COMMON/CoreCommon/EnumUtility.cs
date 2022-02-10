using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ropal.CoreCommon
{
    public static class EnumUtility
    {
        static public class EnumHelper<T>
        {
            static public T Parse(string value)
            {
                return (T)Enum.Parse(typeof(T), value);
            }
        }
    }
}
