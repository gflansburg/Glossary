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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Lists;
using Gafware.Modules.Glossary.Components;
using DotNetNuke.Common;
using DotNetNuke.Security;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Entities.Tabs;

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
    public partial class View : GlossaryModuleBase, IActionable
    {
        public class PageInfo
        {
            public string Url { get; set; }
            public string Title { get; set; }
        }

        protected bool IsGlossary
        {
            get
            {
                return (ViewState["IsGlossary"] != null ? (bool)ViewState["IsGlossary"] : true);
            }
            set
            {
                ViewState["IsGlossary"] = value;
            }
        }

        protected int GlossaryID
        {
            get
            {
                return (ViewState["GlossaryID"] != null ? (int)ViewState["GlossaryID"] : 0);
            }
            set
            {
                ViewState["GlossaryID"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            //System.Web.UI.ScriptManager.GetCurrent(this.Page).EnableHistory = true;
            //System.Web.UI.ScriptManager.GetCurrent(this.Page).SupportsPartialRendering = true;
            base.OnInit(e);
        }

        protected void Page_PreRender(System.Object sender, System.EventArgs e)
        {
            if (Request.IsAuthenticated && pnlSearch.Visible)
            {
                if (DotNetNuke.Security.Permissions.ModulePermissionController.CanEditModuleContent(this.ModuleConfiguration))
                {
                    topIconBar.Visible = true;
                }
                else if (IsEditable)
                {
                    topIconBar.Visible = true;
                }
                else
                {
                    topIconBar.Visible = false;
                }
            }
            else
            {
                topIconBar.Visible = false;
            }
        }

        /// <summary>
        /// Page_Load runs when the control is loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                /*if (DotNetNuke.Framework.AJAX.IsInstalled())
                {
                    DotNetNuke.Framework.AJAX.RegisterScriptManager();
                    System.Web.UI.ScriptManager.GetCurrent(this.Page).Navigate += new EventHandler<HistoryEventArgs>(RadScriptManager_Navigate);
                }*/
                if (!Page.IsPostBack)
                {
                    ScheduleJob();
                    BindDropDown();
                    ProcessQueryString();
                    icons.ModuleId = ModuleId;
                    icons.EditCategoryUrl = EditUrl("EditCategory");
                    icons.EditGlossaryUrl = EditUrl("Edit");
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc, true);
            }
        }

        private void BindDropDown()
        {
            mCategoryDropDownList.DataSource = GlossaryController.ListGlossaryCategoryDefinitions(PortalId);
            mCategoryDropDownList.DataBind();
            mCategoryDropDownList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Categories", ""));
            mCategoryDropDownList.SelectedIndex = 0;
        }

        private void ProcessQueryString()
        {
            mCategoryDropDownList.SelectedIndex = 0;
            rowCategory.Visible = rowIn.Visible = CategoryVisible;
            if (mCategoryDropDownList.Items.FindByValue(DefaultCategory) != null)
            {
                mCategoryDropDownList.SelectedValue = DefaultCategory;
            }
            if (Request.Params["Letter"] != null)
            {
                letterFilter.Filter = Request.Params["Letter"];
            }
            if (Request.Params["Term"] != null && Request.Params["Category"] != null)
            {
                mFilterTextBox.Text = Request.Params["Term"];
                if (mCategoryDropDownList.Items.FindByValue(Request.Params["Category"]) != null)
                {
                    mCategoryDropDownList.SelectedValue = Request.Params["Category"];
                }
            }
            else if (Request.Params["Category"] != null)
            {
                if (mCategoryDropDownList.Items.FindByValue(Request.Params["Category"]) != null)
                {
                    mCategoryDropDownList.SelectedValue = Request.Params["Category"];
                }
            }
            else if (Request.Params["Term"] != null)
            {
                mFilterTextBox.Text = Request.Params["Term"];
            }
            IsGlossary = (Request.Params["Term"] == null);
            BindSearch();
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("AddContent", LocalResourceFile), ModuleActionType.AddContent, "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        },
                        {
                            GetNextActionID(), Localization.GetString("AddCategory", LocalResourceFile), ModuleActionType.AddContent, "", "",
                            EditUrl("EditCategory"), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

        protected void mSearchButton_Click(object sender, EventArgs e)
        {
            BindSearch();
        }

        protected void letterFilter_Click(object sender, LetterFilter.LetterFilterEventArgs e)
        {
            BindSearch();
        }

        private void BindSearch()
        {
            List<GlossaryInfo> glossary = null;
            Index = String.Empty;
            if (!String.IsNullOrEmpty(mFilterTextBox.Text))
            {
                if (mCategoryDropDownList.SelectedIndex > 0)
                {
                    glossary = GlossaryController.ListGlossaryByFilterAndCategory(PortalId, mFilterTextBox.Text, Convert.ToInt32(mCategoryDropDownList.SelectedValue));
                }
                else if (IsPostBack)
                {
                    glossary = GlossaryController.ListGlossaryByFilter(PortalId, mFilterTextBox.Text);
                }
                else
                {
                    glossary = GlossaryController.ListGlossaryByTerm(PortalId, mFilterTextBox.Text);
                }
            }
            else if (mCategoryDropDownList.SelectedIndex > 0)
            {
                glossary = GlossaryController.ListGlossaryByCategory(PortalId, Convert.ToInt32(mCategoryDropDownList.SelectedValue));
            }
            else
            {
                glossary = GlossaryController.ListGlossary(PortalId);
            }
            List<string> letters = new List<string>();
            if (glossary.Find(p => p.Term.StartsWith("1") || p.Term.StartsWith("2") || p.Term.StartsWith("3") || p.Term.StartsWith("4") || p.Term.StartsWith("5") || p.Term.StartsWith("6") || p.Term.StartsWith("7") || p.Term.StartsWith("8") || p.Term.StartsWith("9") || p.Term.StartsWith("0")) != null)
            {
                letters.Add("#");
            }
            foreach (GlossaryInfo item in glossary)
            {
                if(letters.Find(p => p.Equals(item.Term.Substring(0, 1).ToUpper())) == null)
                {
                    letters.Add(item.Term.Substring(0, 1).ToUpper());
                }
            }
            letters.Add("All");
            letterFilter.Letters = letters.ToArray();
            if (!letterFilter.Filter.Equals("All"))
            {
                if (letterFilter.Filter.Equals("#"))
                {
                    glossary = glossary.FindAll(p => p.Term.StartsWith("1") || p.Term.StartsWith("2") || p.Term.StartsWith("3") || p.Term.StartsWith("4") || p.Term.StartsWith("5") || p.Term.StartsWith("6") || p.Term.StartsWith("7") || p.Term.StartsWith("8") || p.Term.StartsWith("9") || p.Term.StartsWith("0"));
                }
                else
                {
                    glossary = glossary.FindAll(p => p.Term.ToUpper().StartsWith(letterFilter.Filter));
                }
            }
            rptGlossary.DataSource = glossary;
            rptGlossary.DataBind();
            if (glossary.Count == 1 && !String.IsNullOrEmpty(mFilterTextBox.Text) && !IsPostBack)
            {
                GlossaryID = glossary[0].GlossaryId;
                ShowDetails(GlossaryID);
                /*if (IsPostBack)
                {
                    AddHistoryPoint();
                }*/
            }
        }

        private string Index { get; set; }

        protected void rptGlossary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl header = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("header");
                GlossaryInfo item = (GlossaryInfo)e.Item.DataItem;
                if (!item.Term.ToUpper().StartsWith(Index) || String.IsNullOrEmpty(Index))
                {
                    Index = item.Term.Substring(0, 1).ToUpper();
                    header.InnerText = Index;
                    header.Visible = true;
                }
            }
        }

        protected void lnkDetails_Command(object sender, CommandEventArgs e)
        {
            GlossaryID = Convert.ToInt32(e.CommandArgument);
            ShowDetails(GlossaryID);
            //AddHistoryPoint();
        }
 
        private void ShowDetails(int glossaryId)
        {
            lnkBack.Visible = IsPostBack;
            lnkBack2.Visible = !IsPostBack;
            pnlDetails.Visible = true;
            pnlSearch.Visible = false;
            GlossaryInfo item = GlossaryController.GetGlossary(glossaryId);
            lblDefinition.Text = item.Definition;
            lblTitle.Text = item.Term;
            //lblDate.Text = "Last modified on " + item.LastModifiedOnDate.ToString("MM/dd/yyyy");
            rptAltTerms.DataSource = item.AltTerms;
            rptAltTerms.DataBind();
            divAKA.Visible = (item.AltTerms.Count > 0);
            List<PageInfo> pages = new List<PageInfo>();
            foreach (GlossaryPage page in item.Pages)
            {
                var tabInfo = TabController.Instance.GetTab(page.TabId, PortalId);
                if (tabInfo != null)
                {
                    DotNetNuke.Security.Permissions.TabPermissionCollection tpc = tabInfo.TabPermissions;
                    string authorizedRoles = tpc.ToString("VIEW");
                    if(DotNetNuke.Security.PortalSecurity.IsInRoles(authorizedRoles))
                    {
                        PageInfo p = new PageInfo();
                        p.Url = tabInfo.FullUrl;
                        p.Title = String.IsNullOrEmpty(tabInfo.Title) ? tabInfo.TabName : tabInfo.Title;
                        pages.Add(p);
                    }
                }
            }
            rptPages.DataSource = pages;
            rptPages.DataBind();
            divAppearsOn.Visible = (pages.Count > 0);
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            pnlDetails.Visible = false;
            pnlSearch.Visible = true;
            GlossaryID = 0;
            if (!IsGlossary)
            {
                mFilterTextBox.Text = String.Empty;
                mCategoryDropDownList.SelectedIndex = 0;
                BindSearch();
            }
            IsGlossary = true;
            //AddHistoryPoint();
        }

        protected void ScheduleJob()
        {
            ScheduleItem item = SchedulingProvider.Instance().GetSchedule(GlossaryJob.AssemblyName, String.Empty);
            if (item == null)
            {
                item = new ScheduleItem();
                item.CatchUpEnabled = false;
                item.Enabled = true;
                item.NextStart = DateTime.Now.AddDays(1);
                item.RetainHistoryNum = 60;
                item.RetryTimeLapse = 1;
                item.RetryTimeLapseMeasurement = "h";
                item.TimeLapse = 6;
                item.TimeLapseMeasurement = "h";
                item.ScheduleSource = ScheduleSource.NOT_SET;
                item.TypeFullName = GlossaryJob.AssemblyName;
                item.FriendlyName = "Glossary: Site Crawler";
                SchedulingProvider.Instance().AddSchedule(item);
            }
        }

        protected void mCategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSearch();
        }

        /*protected void RadScriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            string indexString = e.State["data"];
            if (!String.IsNullOrEmpty(indexString))
            {
                HistoryPoint historyPoint = (HistoryPoint)HR.OU.EDU.Xml.SerializerHelper.FromXml(indexString, typeof(HistoryPoint));
                GlossaryID = historyPoint.GlossaryID;
                mCategoryDropDownList.SelectedIndex = historyPoint.CategoryID;
                mFilterTextBox.Text = historyPoint.Filter;
                pnlDetails.Visible = (GlossaryID > 0);
                pnlSearch.Visible = (GlossaryID == 0);
                letterFilter.Filter = historyPoint.Letter;
                if (GlossaryID > 0)
                {
                    ShowDetails(GlossaryID);
                }
            }
            else
            {
                pnlDetails.Visible = false;
                pnlSearch.Visible = true;
                mCategoryDropDownList.SelectedIndex = 0;
                GlossaryID = 0;
                letterFilter.Filter = "All";
                mFilterTextBox.Text = String.Empty;
                ProcessQueryString();
            }
        }
        
        private void AddHistoryPoint()
        {
            if (!System.Web.UI.ScriptManager.GetCurrent(this.Page).IsNavigating)
            {
                HistoryPoint historyPoint = new HistoryPoint();
                historyPoint.GlossaryID = GlossaryID;
                historyPoint.Filter = mFilterTextBox.Text;
                historyPoint.CategoryID = mCategoryDropDownList.SelectedIndex;
                historyPoint.Letter = letterFilter.Filter;
                string xml = HR.OU.EDU.Xml.SerializerHelper.ToXml(historyPoint);
                System.Web.UI.ScriptManager.GetCurrent(this.Page).AddHistoryPoint("data", xml);
            }
        }*/

    }

    /*[Serializable]
    [XmlType("HistoryPoint")]
    public class HistoryPoint
    {
        public int GlossaryID { get; set; }
        public int CategoryID { get; set; }
        public string Filter { get; set; }
        public string Letter { get; set; }
    }*/
}