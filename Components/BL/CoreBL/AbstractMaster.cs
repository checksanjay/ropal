using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml.Linq;

namespace Ropal.CoreBL
{
    public abstract class AbstractMaster : MasterPage
    {        
        public string s_className;

        protected virtual void OnPreInit(EventArgs e)
        {

        }
      

        protected virtual void OnInit(EventArgs e)
        {
          s_className = MethodBase.GetCurrentMethod().DeclaringType.Name;
        }
                
        protected virtual void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PrepareMasterPage();
        }
        
        public void PrepareMasterPage()
        {
           
        }
    }
}
