using System;
using System.Collections.Generic;
using System.Linq;
using Gafware.Modules.Glossary.Components;
using Gafware.Net;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Common.Utilities;

namespace Gafware.Modules.Glossary
{
    public class GlossaryJob : SchedulerClient 
    {
        public const string AssemblyName = "Gafware.Modules.Glossary.GlossaryJob,Gafware.Glossary";
        
        public GlossaryJob (ScheduleHistoryItem oItem) : base()
        {
            this.ScheduleHistoryItem = oItem;
        }

         public override void DoWork()
        {
            try
            {
                //Perform required items for logging
                this.Progressing();

                //Your code goes here
                PortalController portalController = new PortalController();
                PortalAliasController paController = new PortalAliasController();
                List<PortalInfo> portals = portalController.GetPortals().Cast<PortalInfo>().ToList();
                this.ScheduleHistoryItem.AddLogNote("Checking " + portals.Count.ToString() + " Portals<br />\r\n");
                foreach (PortalInfo portal in portals)
                {
                    this.ScheduleHistoryItem.AddLogNote("<br />\r\nChecking Portal: " + portal.PortalName + "<br />\r\n");
                    string portalAlias = String.Empty;
                    foreach (var pa in paController.GetPortalAliasesByPortalId(portal.PortalID))
                    {
                        if (pa.IsPrimary)
                        {
                            portalAlias = pa.HTTPAlias;
                            break;
                        }
                    }
                    if (!String.IsNullOrEmpty(portalAlias))
                    {
                        this.ScheduleHistoryItem.AddLogNote("Crawling Website: http://" + portalAlias + "<br />\r\n");
                        List<TabInfo> tabs = GetPortalTabs(portal.PortalID, true);
                        tabs = tabs.FindAll(p => !p.IsSuperTab && !p.PermanentRedirect && !String.IsNullOrEmpty(p.TabPath) && !p.TabPath.StartsWith("//ActivityFeed//", StringComparison.OrdinalIgnoreCase) && !p.TabPath.StartsWith("//Admin//", StringComparison.OrdinalIgnoreCase) && !p.TabPath.Equals("//ActivityFeed", StringComparison.OrdinalIgnoreCase) && !p.TabPath.Equals("//Admin", StringComparison.OrdinalIgnoreCase));
                        foreach (var tab in tabs)
                        {
                            try
                            {
                                List<GlossaryPage> pages = GlossaryController.ListGlossaryPagesByTab(tab.TabID);
                                string url = "http://" + portalAlias + tab.TabPath.Replace("//", "/") + ".aspx";
                                this.ScheduleHistoryItem.AddLogNote("Searching Page: " + url);
                                if (!tab.IsDeleted)
                                {
                                    string html = GetUrl(url);
                                    if (html.Length > 0)
                                    {
                                        this.ScheduleHistoryItem.AddLogNote(" (" + html.Length + " bytes)<br />\r\n");
                                        HtmlDocument doc = new HtmlDocument(html, false);
                                        ProcessNodes(doc.Nodes, tab, pages);
                                        foreach (GlossaryPage page in pages)
                                        {
                                            GlossaryInfo info = GlossaryController.GetGlossary(page.GlossaryId);
                                            if (!TermInTab(doc.Nodes, info))
                                            {
                                                GlossaryController.DeleteGlossaryPage(page.PageId);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (GlossaryPage page in pages)
                                        {
                                            GlossaryController.DeleteGlossaryPage(page.PageId);
                                        }
                                    }
                                }
                                else
                                {
                                    this.ScheduleHistoryItem.AddLogNote("...Page Marked For Deletion<br />\r\n");
                                    foreach (GlossaryPage page in pages)
                                    {
                                        GlossaryController.DeleteGlossaryPage(page.PageId);
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                this.ScheduleHistoryItem.AddLogNote("<br />\r\nException= " + ex.ToString() + "<br />\r\n");
                            }
                        }
                        try
                        {
                            List<GlossaryPage> pages = GlossaryController.ListGlossaryPagesByPortal(portal.PortalID);
                            foreach (GlossaryPage page in pages)
                            {
                                var tab = tabs.First(p => p.TabID == page.TabId);
                                if (tab == null)
                                {
                                    GlossaryController.DeleteGlossaryPage(page.PageId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.ScheduleHistoryItem.AddLogNote("<br />\r\nException= " + ex.ToString() + "<br />\r\n");
                        }
                    }
                }
                try
                {
                    List<GlossaryInfo> terms = GlossaryController.ListAllGlossary();
                    foreach (GlossaryInfo term in terms)
                    {
                        PortalInfo portal = portals.Find(p => p.PortalID == term.PortalId);
                        if (portal == null)
                        {
                            GlossaryController.DeleteGlossary(term.GlossaryId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ScheduleHistoryItem.AddLogNote("<br />\r\nException= " + ex.ToString() + "<br />\r\n");
                }
                this.ScheduleHistoryItem.AddLogNote("<br />\r\nFinished<br />\r\n");
                this.ScheduleHistoryItem.Succeeded = true;
            }
            catch (Exception ex)
            {
                this.ScheduleHistoryItem.Succeeded = false;
                this.ScheduleHistoryItem.AddLogNote("Exception= " + ex.ToString());
                this.Errored(ref ex);
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
        }

         private bool TermInTab(HtmlNodeCollection nodes, GlossaryInfo info)
         {
             foreach (HtmlNode node in nodes)
             {
                if (node.GetType() == typeof(HtmlElement))
                {
                    HtmlElement element = (HtmlElement)node;
                    if (element.Name.Equals("a", StringComparison.OrdinalIgnoreCase))
                    {
                        if (element.Attributes["class"] != null && element.Attributes["class"].Value.Equals("glossaryitem", StringComparison.OrdinalIgnoreCase))
                        {
                            string term = System.Web.HttpUtility.HtmlDecode(element.Text);
                            if (term.Equals(info.Term, StringComparison.OrdinalIgnoreCase) || info.AltTerms.Find(p => p.Term.Equals(term, StringComparison.OrdinalIgnoreCase)) != null)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (TermInTab(element.Nodes, info))
                        {
                            return true;
                        }
                    }
                }
             }
             return false;
         }

        private void ProcessNodes(HtmlNodeCollection nodes, TabInfo tab, List<GlossaryPage> pages)
        {
            foreach (HtmlNode node in nodes)
            {
                if (node.GetType() == typeof(HtmlElement))
                {
                    HtmlElement element = (HtmlElement)node;
                    if (element.Name.Equals("a", StringComparison.OrdinalIgnoreCase))
                    {
                        if (element.Attributes["class"] != null && element.Attributes["class"].Value.Equals("glossaryitem", StringComparison.OrdinalIgnoreCase))
                        {
                            string term = System.Web.HttpUtility.HtmlDecode(element.Text);
                            this.ScheduleHistoryItem.AddLogNote("Found Term: " + System.Web.HttpUtility.HtmlEncode(term) + "<br />\r\n");
                            List<GlossaryInfo> info = GlossaryController.ListGlossaryByFilter(tab.PortalID, term);
                            if (info != null && info.Count > 0)
                            {
                                GlossaryPage page = pages.Find(p => p.GlossaryId == info[0].GlossaryId);
                                if (page == null)
                                {
                                    page = new GlossaryPage();
                                    page.Term = term;
                                    page.TabId = tab.TabID;
                                    page.GlossaryId = info[0].GlossaryId;
                                    GlossaryController.SaveGlossaryPage(page);
                                    pages.Add(page);
                                }
                            }
                        }
                    }
                    else
                    {
                        ProcessNodes(element.Nodes, tab, pages);
                    }
                }
            }
        }

        private string GetUrl(string url)
        {
            return GetUrl(url, String.Empty, 0);
        }

        private string GetUrl(string url, string proxyServer, int proxyPort)
        {
            string strRet = "";
            try
            {
                byte[] data = GetUrlFile(url, proxyServer, proxyPort);
                if (data != null && data.Length > 0)
                {
                    strRet = System.Text.ASCIIEncoding.ASCII.GetString(data);
                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    this.ScheduleHistoryItem.AddLogNote("...Page Not Found<br />\r\n");
                }
                else
                {
                    this.ScheduleHistoryItem.AddLogNote("<br />\r\nException= " + ex.ToString());
                }
            }
            return strRet;
        }

        private byte[] GetUrlFile(string url, string proxyServer, int proxyPort)
        {
            byte[] data = null;
            try
            {
                Uri objURI = new Uri(url);
                if (objURI != null)
                {
                    System.Net.HttpWebRequest objWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(objURI);
                    objWebRequest.CookieContainer = new System.Net.CookieContainer();
                    if (proxyServer.Length > 0)
                    {
                        objWebRequest.Proxy = new System.Net.WebProxy(proxyServer, proxyPort);
                    }
                    objWebRequest.Timeout = 100000;
                    objWebRequest.Headers.Add("Accept-Language", "en-us");
                    objWebRequest.Accept = "*/*";
                    objWebRequest.AllowAutoRedirect = true;
                    objWebRequest.KeepAlive = true;
                    objWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.1)";
                    System.Net.WebResponse objWebResponse = objWebRequest.GetResponse();
                    System.IO.Stream objStream = objWebResponse.GetResponseStream();
                    System.IO.BinaryReader objStreamReader = new System.IO.BinaryReader(objStream);
                    if (objStreamReader != null)
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        if (ms != null)
                        {
                            byte[] buf = new byte[1024];
                            int count;
                            while ((count = objStreamReader.Read(buf, 0, 1024)) > 0)
                            {
                                ms.Write(buf, 0, count);
                            }
                            ms.Position = 0;
                            data = new byte[ms.Length];
                            ms.Read(data, 0, data.Length);
                            ms.Close();
                        }
                        objStreamReader.Close();
                    }
                    objStream.Close();
                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    this.ScheduleHistoryItem.AddLogNote("...Page Not Found<br />\r\n");
                }
                else
                {
                    this.ScheduleHistoryItem.AddLogNote("<br />\r\nException= " + ex.ToString());
                }
            }
            return data;
        }

        public static List<TabInfo> GetTabsBySortOrder(int portalId, string cultureCode, bool includeNeutral)
        {
            return new TabController().GetTabsByPortal(portalId).WithCulture(cultureCode, includeNeutral).AsList();
        }

        public static List<TabInfo> GetPortalTabs(int portalId, bool includeHidden)
        {
            List<TabInfo> tabs = GetTabsBySortOrder(portalId, PortalController.GetActivePortalLanguage(portalId), true);
            var listTabs = new List<TabInfo>();
            foreach (TabInfo objTab in tabs)
            {
                if ((objTab.IsVisible || includeHidden) && (objTab.IsDeleted == false) && (objTab.TabType == TabType.Normal))
                {
                    listTabs.Add(objTab);
                }
            }
            return listTabs;
        }
    }
}
