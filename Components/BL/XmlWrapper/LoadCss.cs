using Ropal;
using Ropal.CoreConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ropal.CoreXmlWrapper
{
    [Serializable]
    public class LoadCss
    {        
        public List<PageName> pageNames;
    }

    [Serializable]
    public class PageName
    {
        public string name { get; set; }
        //public IEnumerable<CssFileLocation> Files { get; set; }
        public List<CssFileLocation> Files { get; set; }

    }

    [Serializable]
    public class CssFileLocation
    {
        public string location { get; set; }   
        public CssFileLocation(string s_loc)
        {
            location = s_loc;
        }
             
    }

}
