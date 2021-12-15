using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gafware.Modules.Glossary
{
    public partial class Icons : System.Web.UI.UserControl
    {
        public int ModuleId
        {
            get
            {
                return (ViewState["ModuleId"] != null ? (int)ViewState["ModuleId"] : -1);
            }
            set
            {
                ViewState["ModuleId"] = value;
            }
        }

        public string EditGlossaryUrl
        {
            get
            {
                return btnGlossary.NavigateUrl;
            }
            set
            {
                btnGlossary.NavigateUrl = value;
            }
        }

        public string EditCategoryUrl
        {
            get
            {
                return btnCategory.NavigateUrl;
            }
            set
            {
                btnCategory.NavigateUrl = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}