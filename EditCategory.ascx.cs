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
using DotNetNuke.Abstractions;
using DotNetNuke.Services.Exceptions;
using Gafware.Modules.Glossary.Components;
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
    public partial class EditCategory : GlossaryModuleBase
    {
        private readonly INavigationManager _navigationManager;

        public EditCategory()
        {
            _navigationManager = DependencyProvider.GetRequiredService<INavigationManager>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
            mCategoryGridView.DataSource = GlossaryController.ListGlossaryCategoryDefinitions(PortalId);
            mCategoryGridView.DataBind();
        }

        protected void mCategoryGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            mCategoryGridView.EditIndex = e.NewEditIndex;
            BindData();
        }

        protected void mCategoryGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = mCategoryGridView.EditIndex;
            GridViewRow row = mCategoryGridView.Rows[index];
            TextBox categoryName = row.FindControl("tbCategoryName") as TextBox;
            List<GlossaryCategoryDefinition> glossaryCategoryDefiniions = GlossaryController.ListGlossaryCategoryDefinitions(PortalId);
            GlossaryCategoryDefinition glossaryCategoryDefinition = glossaryCategoryDefiniions.Find(p => p.GlossaryCategoryDefinitionId == (int)mCategoryGridView.DataKeys[index].Value);
            if (glossaryCategoryDefinition != null)
            {
                glossaryCategoryDefinition.PortalId = PortalId;
                glossaryCategoryDefinition.CategoryName = categoryName.Text;
                GlossaryController.SaveGlossaryCategoryDefinition(glossaryCategoryDefinition);
            }
            mCategoryGridView.EditIndex = -1;
            mCategoryGridView.DataSource = glossaryCategoryDefiniions;
            mCategoryGridView.DataBind();
        }

        protected void mCategoryGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            mCategoryGridView.EditIndex = -1;
            BindData();
        }

        protected void mCategoryGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox categoryName = e.Row.FindControl("tbCategoryName") as TextBox;
                if (categoryName != null)
                {
                    ImageButton saveButton = e.Row.FindControl("saveButton") as ImageButton;
                    string js = "if ((event.which && event.which == 13) || "
                                + "(event.keyCode && event.keyCode == 13)) "
                                + "{" + Page.ClientScript.GetPostBackEventReference(saveButton, String.Empty) + ";return false;} "
                                + "else return true;";
                    categoryName.Attributes.Add("onkeydown", js);
                }
            }
            else if(e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox categoryName = e.Row.FindControl("tbCategoryName2") as TextBox;
                if (categoryName != null)
                {
                    ImageButton newButton = e.Row.FindControl("saveButton2") as ImageButton;
                    string js = "if ((event.which && event.which == 13) || "
                                + "(event.keyCode && event.keyCode == 13)) "
                                + "{" + Page.ClientScript.GetPostBackEventReference(newButton, String.Empty) + ";return false;} "
                                + "else return true;";
                    categoryName.Attributes.Add("onkeydown", js);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }
        }

        protected void mCategoryGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int glossaryCategoryDefinitionId = (int)mCategoryGridView.DataKeys[e.RowIndex].Value;
            GlossaryController.DeleteGlossaryCategoryDefinition(glossaryCategoryDefinitionId);
            BindData();
        }

        protected void mCategoryGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
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
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = true;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton2") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton2") as ImageButton;
                    newButton.Visible = false;
                    saveButton.Visible = cancelButton.Visible = true;
                }
                else if (e.CommandName.Equals("Cancel"))
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
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = false;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton2") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton2") as ImageButton;
                    newButton.Visible = true;
                    saveButton.Visible = cancelButton.Visible = false;
                }
                else if (e.CommandName.Equals("Insert"))
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
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        row.Cells[i].Visible = false;
                    }
                    ImageButton newButton = row.FindControl("newButton") as ImageButton;
                    ImageButton saveButton = row.FindControl("saveButton2") as ImageButton;
                    ImageButton cancelButton = row.FindControl("cancelButton2") as ImageButton;
                    newButton.Visible = true;
                    saveButton.Visible = cancelButton.Visible = false;
                    TextBox tbCategoryName = row.FindControl("tbCategoryName2") as TextBox;
                    GlossaryCategoryDefinition glossaryCategoryDefinition = new GlossaryCategoryDefinition();
                    glossaryCategoryDefinition.PortalId = PortalId;
                    glossaryCategoryDefinition.CategoryName = tbCategoryName.Text;
                    GlossaryController.SaveGlossaryCategoryDefinition(glossaryCategoryDefinition);
                    BindData();
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
    }
}