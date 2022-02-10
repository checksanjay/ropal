using Ropal.CoreConstants;
using Ropal.CoreXmlWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlWrapper
{
    public static class LoadCssExtend
    {
        public static LoadCss GetLoadCssObject(IEnumerable<XElement> c_Nodes)
        {
            LoadCss o_LoadCss = new LoadCss();
            List<PageName> o_pageNames = new List<PageName>();
            foreach (XElement node in c_Nodes)
            {
                PageName o_pageName = new PageName();
                List<CssFileLocation> s_locations = new List<CssFileLocation>();
                o_pageName.name = ((string)node.Attribute(XmlWrapperConstants.name));

                foreach (XElement childNode in node.Nodes())
                {
                    string s_loc = ((string)childNode.Attribute(XmlWrapperConstants.location));
                    if (!String.IsNullOrEmpty(s_loc))
                    {
                        s_locations.Add(new CssFileLocation(s_loc));
                    }
                }

                if (s_locations != null && s_locations.Count > 0)
                    o_pageName.Files = s_locations;

                o_pageNames.Add(o_pageName);
            }
            //o_LoadCss.pageNames = o_pageNames.AsEnumerable();
            o_LoadCss.pageNames = o_pageNames;
            return o_LoadCss;
        }
    }
}
