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
using System.Collections.Generic;
using Gafware.Modules.Glossary.Components;
using Gafware.Modules.Glossary.Data;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

namespace Gafware.Modules.Glossary
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from GlossarySettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class AgentSettings : GlossaryModuleSettingsBase
    {
        #region Base Method Implementations
        private const string c_jqImagePicker = "jquery.plugin.imagepicker";

        private void ImportPlugins()
        {
            //load the plugin client scripts on every page load
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqImagePicker))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqImagePicker, String.Format(GlossaryController.SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/image-picker.min.js")), false);
            }
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl(String.Format(GlossaryController.CSS_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/image-picker.css"))));
        }

        public class Icon
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        private void BindIcons(bool reload)
        {
            List<Icon> icons = new List<Icon>();
            string[] files = System.IO.Directory.GetFiles(MapPath(ResolveUrl("Images/Icons")), "*.svg");
            foreach (string file in files)
            {
                Icon icon = new Icon();
                icon.Text = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(System.IO.Path.GetFileNameWithoutExtension(file).Replace("_", " ").ToLower());
                icon.Value = ResolveUrl("Images/Icons/" + System.IO.Path.GetFileName(file));
                icons.Add(icon);
            }
            if (reload)
            {
                ddlImage.DataSource = icons;
                ddlImage.DataBind();
                ddlImage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Icon --", String.Empty));
                ddlImage.SelectedIndex = 0;
            }
            foreach (System.Web.UI.WebControls.ListItem item in ddlImage.Items)
            {
                Icon icon = icons.Find(p => p.Value.Equals(item.Value, StringComparison.OrdinalIgnoreCase));
                if (icon != null)
                {
                    item.Attributes.Add("data-img-src", icon.Value);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsPostBack)
            {
                BindIcons(false);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                ImportPlugins();
                if (Page.IsPostBack == false)
                {
                    BindIcons(true);
                    ctlGlossaryPage.Url = GlossaryPage;
                    lstStyle.SelectedValue = ToolTipStyle;
                    chkRounded.Checked = Rounded;
                    chkShadowed.Checked = Shadowed;
                    lstPostionAtX.SelectedValue = PositionAtX;
                    lstPostionAtY.SelectedValue = PositionAtY;
                    lstPostionMyX.SelectedValue = PositionMyX;
                    lstPostionMyY.SelectedValue = PositionMyY;
                    chkUseImage.Checked = UseImage;
                    ddlImage.SelectedIndex = ddlImage.Items.IndexOf(ddlImage.Items.FindByValue(ImageUrl));
                    pnlImage.Visible = chkUseImage.Checked;
                    cpLinkColor.SelectedColor = LinkColor;
                    cpHoverColor.SelectedColor = HoverColor;
                    cpUnderlineColor.SelectedColor = UnderlineColor;
                    cpUnderlineHoverColor.SelectedColor = UnderlineHoverColor;
                    lstShow.SelectedValue = ShowEvent;
                    lstHide.SelectedValue = HideEvent;
                    lstUnderline.SelectedValue = UnderlineStyle;
                    lstUnderlineWidth.SelectedValue = UnderlineWidth;
                    lstLinkBold.SelectedValue = LinkBold;
                    chkFriendlyUrl.Checked = UseFriendlyUrl;
                }
                else
                {
                    BindIcons(false);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();

                //module settings
                modules.UpdateModuleSetting(ModuleId, GlossaryController.GLOSSARY_PAGE_KEY, ctlGlossaryPage.Url);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.STYLE_KEY, lstStyle.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.ROUNDED_KEY, chkRounded.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, GlossaryController.SHADOWED_KEY, chkShadowed.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, GlossaryController.POSITION_AT_X_KEY, lstPostionAtX.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.POSITION_AT_Y_KEY, lstPostionAtY.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.POSITION_MY_X_KEY, lstPostionMyX.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.POSITION_MY_Y_KEY, lstPostionMyY.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.USE_IMAGE_KEY, chkUseImage.Checked.ToString());
                modules.UpdateModuleSetting(ModuleId, GlossaryController.IMAGE_KEY, ddlImage.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.LINK_COLOR_KEY, System.Drawing.ColorTranslator.ToHtml(cpLinkColor.SelectedColor));
                modules.UpdateModuleSetting(ModuleId, GlossaryController.HOVER_COLOR_KEY, System.Drawing.ColorTranslator.ToHtml(cpHoverColor.SelectedColor));
                modules.UpdateModuleSetting(ModuleId, GlossaryController.UNDERLINE_COLOR_KEY, System.Drawing.ColorTranslator.ToHtml(cpUnderlineColor.SelectedColor));
                modules.UpdateModuleSetting(ModuleId, GlossaryController.UNDERLINE_HOVER_COLOR_KEY, System.Drawing.ColorTranslator.ToHtml(cpUnderlineHoverColor.SelectedColor));
                modules.UpdateModuleSetting(ModuleId, GlossaryController.SHOW_KEY, lstShow.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.HIDE_KEY, lstHide.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.LINK_UNDERLINE_KEY, lstUnderline.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.UNDERLINE_WIDTH_KEY, lstUnderlineWidth.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.LINK_BOLD_KEY, lstLinkBold.SelectedValue);
                modules.UpdateModuleSetting(ModuleId, GlossaryController.USE_FRIENDLY_URL_KEY, chkFriendlyUrl.Checked.ToString());
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        protected void chkUseImage_CheckedChanged(object sender, EventArgs e)
        {
            pnlImage.Visible = chkUseImage.Checked;
        }
    }
}