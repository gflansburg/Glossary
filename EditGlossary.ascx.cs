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
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Lists;
using Gafware.Modules.Glossary.Components;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gafware.Modules.Glossary
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditGlossary class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from GlossaryModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class EditGlossary : GlossaryModuleBase
    {
        private const string c_jqTransformKey = "jquery.plugin.jqtransform";

        private readonly INavigationManager _navigationManager;

        public EditGlossary()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        private void ImportPlugins()
        {
            //load the plugin client scripts on every page load
            if (!Page.ClientScript.IsClientScriptBlockRegistered(c_jqTransformKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), c_jqTransformKey, String.Format(GlossaryController.SCRIPT_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jqTransform/jquery.jqtransform.js")), false);
            }
            Page.Header.Controls.Add(new System.Web.UI.LiteralControl(String.Format(GlossaryController.CSS_TAG_INCLUDE_FORMAT, String.Concat(this.ControlPath, "Scripts/jqTransform/jqtransform.css"))));
        }

        protected int GlossaryId
        {
            get
            {
                return (ViewState["GlossaryId"] != null ? (int)ViewState["GlossaryId"] : 0);
            }
            set
            {
                ViewState["GlossaryId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ImportPlugins();
                if (!IsPostBack)
                {
                    BindData();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void BindData()
        {
            mTermGridView.DataSource = GlossaryController.ListGlossary(PortalId);
            mTermGridView.DataBind();
            lstCategory.DataSource = GlossaryController.ListGlossaryCategoryDefinitions(PortalId);
            lstCategory.DataBind();
        }

        protected string GetCategories(int glossaryId)
        {
            string categories = String.Empty;
            List<GlossaryCategoryDefinition> glossaryCategoryDefinitions = GlossaryController.ListGlossaryCategoryDefinitionsByGlossary(glossaryId);
            foreach (GlossaryCategoryDefinition glossaryCategoryDefinition in glossaryCategoryDefinitions)
            {
                if (!String.IsNullOrEmpty(categories))
                {
                    categories += ", ";
                }
                categories += glossaryCategoryDefinition.CategoryName;
            }
            return categories;
        }

        protected void mTermGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GlossaryId = (int)mTermGridView.DataKeys[e.NewEditIndex].Value;
            e.Cancel = true;
            pnlGrid.Visible = false;
            pnlEdit.Visible = true;
            GlossaryInfo objGlossary = GlossaryController.GetGlossary(GlossaryId);
            txtTerm.Text = objGlossary.Term;
            txtDefinition.Text = System.Web.HttpUtility.HtmlEncode(objGlossary.Definition);
            gvAltTerms.DataSource = objGlossary.AltTerms;
            gvAltTerms.DataBind();
            lstCategory.ClearSelection();
            List<GlossaryCategory> glossaryCategories = GlossaryController.GetGlossaryCategoryByGlossary(GlossaryId);
            foreach (GlossaryCategory glossaryCategory in glossaryCategories)
            {
                lstCategory.Items.FindByValue(glossaryCategory.GlossaryCategoryDefinitionId.ToString()).Selected = true;
            }
        }

        protected void mTermGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int glossaryId = (int)mTermGridView.DataKeys[e.RowIndex].Value;
            GlossaryController.DeleteGlossary(glossaryId);
            BindData();
            e.Cancel = true;
        }

        protected void mTermGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("New"))
                {
                    pnlGrid.Visible = false;
                    pnlEdit.Visible = true;
                    GlossaryId = 0;
                    txtDefinition.Text = String.Empty;
                    txtTerm.Text = String.Empty;
                    gvAltTerms.DataSource = new List<GlossaryAlt>();
                    gvAltTerms.DataBind();
                    lstCategory.ClearSelection();
                }
            }
            catch
            {
            }
        }

        protected void cmdCloseEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect(_navigationManager.NavigateURL());
        }

        protected void cmdCancelEdit_Click(object sender, EventArgs e)
        {
            pnlGrid.Visible = true;
            pnlEdit.Visible = false;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            rfvDefinition.IsValid = !String.IsNullOrEmpty(txtDefinition.Text);
            if (Page.IsValid)
            {
                GlossaryInfo objGlossary = GlossaryController.GetGlossary(GlossaryId);
                if (objGlossary == null)
                {
                    objGlossary = new GlossaryInfo();
                    objGlossary.GlossaryId = GlossaryId;
                    objGlossary.PortalId = PortalId;
                    objGlossary.AltTerms = new List<GlossaryAlt>();
                    objGlossary.Pages = new List<GlossaryPage>();
                }
                objGlossary.Term = txtTerm.Text;
                objGlossary.Definition = System.Web.HttpUtility.HtmlDecode(txtDefinition.Text);
                GlossaryController.SaveGlossary(objGlossary);
                List<GlossaryCategory> glossaryCategories = GlossaryController.GetGlossaryCategoryByGlossary(objGlossary.GlossaryId);
                foreach (ListItem item in lstCategory.Items)
                {
                    if(item.Selected && glossaryCategories.Find(p => p.GlossaryCategoryDefinitionId == Convert.ToInt32(item.Value)) == null)
                    {
                        GlossaryCategory objGlossaryCategory = new GlossaryCategory();
                        objGlossaryCategory.GlossaryCategoryDefinitionId = Convert.ToInt32(item.Value);
                        objGlossaryCategory.GlossaryId = objGlossary.GlossaryId;
                        GlossaryController.SaveGlossaryCategory(objGlossaryCategory);
                    }
                    else if (!item.Selected && glossaryCategories.Find(p => p.GlossaryCategoryDefinitionId == Convert.ToInt32(item.Value)) != null)
                    {
                        GlossaryController.DeleteGlossaryCategory(Convert.ToInt32(item.Value));
                    }
                }
                List<GlossaryAlt> entries = new List<GlossaryAlt>();
                foreach (GridViewRow row in gvAltTerms.Rows)
                {
                    GlossaryAlt item = new GlossaryAlt();
                    item.GlossaryId = objGlossary.GlossaryId;
                    item.Term = ((HiddenField)row.FindControl("hidTerm")).Value;
                    entries.Add(item);
                    if(objGlossary.AltTerms == null || objGlossary.AltTerms.Find(p => p.Term.Equals(item.Term, StringComparison.OrdinalIgnoreCase)) == null)
                    {
                        GlossaryController.SaveGlossaryAlt(item);
                    }
                }
                if (objGlossary.AltTerms != null)
                {
                    foreach (GlossaryAlt altTerm in objGlossary.AltTerms)
                    {
                        if (entries.Find(p => p.Term.Equals(altTerm.Term, StringComparison.OrdinalIgnoreCase)) == null)
                        {
                            GlossaryController.DeleteGlossaryAlt(altTerm.AltId);
                        }
                    }
                }
                pnlGrid.Visible = true;
                pnlEdit.Visible = false;
                BindData();
            }
        }

        protected void gvAltTerms_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<GlossaryAlt> entries = new List<GlossaryAlt>();
            foreach (GridViewRow gvRow in gvAltTerms.Rows)
            {
                if (gvRow.RowIndex != e.RowIndex)
                {
                    GlossaryAlt item = new GlossaryAlt();
                    item.GlossaryId = GlossaryId;
                    item.Term = ((HiddenField)gvRow.FindControl("hidTerm")).Value;
                    entries.Add(item);
                }
            }
            gvAltTerms.DataSource = entries;
            gvAltTerms.DataBind();
            e.Cancel = true;
        }

        protected void gvAltTerms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("New"))
            {
                GridViewRow row = null;
                if (e.CommandSource.GetType() == typeof(LinkButton))
                {
                    LinkButton btnNew = e.CommandSource as LinkButton;
                    row = btnNew.NamingContainer as GridViewRow;
                }
                else if (e.CommandSource.GetType() == typeof(ImageButton))
                {
                    ImageButton btnNew = e.CommandSource as ImageButton;
                    row = btnNew.NamingContainer as GridViewRow;
                }
                if (row == null)
                {
                    return;
                }
                List<GlossaryAlt> entries = new List<GlossaryAlt>();
                foreach (GridViewRow gvRow in gvAltTerms.Rows)
                {
                    GlossaryAlt item = new GlossaryAlt();
                    item.GlossaryId = GlossaryId;
                    item.Term = ((HiddenField)gvRow.FindControl("hidTerm")).Value;
                    entries.Add(item);
                }
                TextBox txtAltTerm = row.FindControl("txtAltTerm") as TextBox;
                if (entries.Find(p => p.Term.Equals(txtAltTerm.Text, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    GlossaryAlt newItem = new GlossaryAlt();
                    newItem.GlossaryId = GlossaryId;
                    newItem.Term = txtAltTerm.Text;
                    entries.Add(newItem);
                    gvAltTerms.DataSource = entries;
                    gvAltTerms.DataBind();
                }
                txtAltTerm.Text = String.Empty;
            }
        }
    }
}