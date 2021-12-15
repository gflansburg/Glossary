/*
' Copyright (c) 2021  Gafware
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Lists;
using Gafware.Modules.Glossary.Components;
using DotNetNuke.Common;
using DotNetNuke.Security;

namespace Gafware.Modules.Glossary
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from GlossaryModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Agent : GlossaryModuleBase, IActionable
    {
        /// <summary>
        /// Page_Load runs when the control is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private const string c_jqTabKey = "jquery.plugin.tabguard";
        private const string c_jqTipKey = "jquery.plugin.qtip";
        private string _imageData = null;

        protected string ImageData
        {
            get
            {
                if (String.IsNullOrEmpty(_imageData))
                {
                    System.IO.FileStream fs = new System.IO.FileStream(MapPath(ImageUrl), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                    fs.Close();
                    _imageData = System.Text.ASCIIEncoding.ASCII.GetString(data).Replace("\r", String.Empty).Replace("\n", String.Empty);
                }
                return (_imageData ?? String.Empty);
            }
        }

        private void ImportPlugins()
        {
            //load the plugin client scripts on every page load
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqTabKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqTabKey, String.Format(GlossaryController.SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jquery.tabguard1.0.js")), false);
            }
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqTipKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqTipKey, String.Format(GlossaryController.SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jquery.qtip.js")), false);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ZZZZZZZ", String.Format(GlossaryController.SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/imagesloaded.pkgd.js")), false);
            }
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl(String.Format(GlossaryController.CSS_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jquery.qtip.css"))));
        }

        protected string ToolTipClasses
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("qtip-glossary");
                if (!String.IsNullOrEmpty(ToolTipStyle))
                {
                    sb.Append(" ");
                    sb.Append(ToolTipStyle);
                }
                if (Rounded)
                {
                    sb.Append(" qtip-rounded");
                }
                if (Shadowed)
                {
                    sb.Append(" qtip-shadow");
                }
                return sb.ToString();
            }
        }

        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                ImportPlugins();
                if (!Page.IsPostBack)
                {
                    if (UseImage && !String.IsNullOrEmpty(ImageUrl) && System.IO.File.Exists(MapPath(ImageUrl)))
                    {
                        litJS.Text = "$(this).after('<a style=\"padding-left: 2px;\" href=\"" + (ShowEvent.Equals("click") ? "javascript:void(null)" : GlossaryPage + "?term=' + escape($(this).text()) + '" ) + "\" alt=\"Definition for ' + escape($(this).text()) + '\">" + ImageData + "</a>');";
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<style type=\"text/css\">");
                    sb.AppendLine("    a.glossaryitem, a.glossaryitem:link, a.glossaryitem:visited {");
                    sb.AppendLine("        color: " + System.Drawing.ColorTranslator.ToHtml(LinkColor) + ";");
                    sb.AppendLine("    }");
                    sb.AppendLine("    a.glossaryitem:hover, a.glossaryitem:focus, a.glossaryitem:active {");
                    sb.AppendLine("        color: " + System.Drawing.ColorTranslator.ToHtml(HoverColor) + ";");
                    sb.AppendLine("    }");
                    sb.AppendLine("    svg path.glossary,");
                    sb.AppendLine("    svg polygon.glossary {");
                    sb.AppendLine("        fill: " + System.Drawing.ColorTranslator.ToHtml(LinkColor) + ";");
                    sb.AppendLine("        stroke: " + System.Drawing.ColorTranslator.ToHtml(LinkColor) + ";");
                    sb.AppendLine("    }");
                    sb.AppendLine("    svg path.glossary:hover,");
                    sb.AppendLine("    svg polygon.glossary:hover,");
                    sb.AppendLine("    svg path.glossary:focus,");
                    sb.AppendLine("    svg polygon.glossary:focus {");
                    sb.AppendLine("        fill: " + System.Drawing.ColorTranslator.ToHtml(HoverColor) + ";");
                    sb.AppendLine("        stroke: " + System.Drawing.ColorTranslator.ToHtml(HoverColor) + ";");
                    sb.AppendLine("    }");
                    sb.AppendLine("    a.glossaryitem, a.glossaryitem:link, a.glossaryitem:visited, a.glossaryitem:active, a.glossaryitem:focus, a.glossaryitem:hover {");
                    sb.AppendLine("        border-bottom-width: " + UnderlineWidth + ";");
                    sb.AppendLine("        border-bottom-style: " + UnderlineStyle + ";");
                    sb.AppendLine("        font-weight: " + LinkBold + " !important;");
                    sb.AppendLine("        text-decoration: none !important;");
                    sb.AppendLine("    }");
                    sb.AppendLine("</style>");
                    litCSS.Text = sb.ToString();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc, true);
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection();
            }
        }
    }
}