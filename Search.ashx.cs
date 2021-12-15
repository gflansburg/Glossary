using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gafware.Modules.Glossary.Components;

namespace Gafware.Modules.Glossary
{
    /// <summary>
    /// Summary description for Search
    /// </summary>
    public class Search : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string term = (context.Request.QueryString["term"] ?? String.Empty);
            int portalId = Convert.ToInt32(context.Request.QueryString["pid"] ?? "0");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (!String.IsNullOrEmpty(term))
            {
                List<GlossaryInfo> glossary = GlossaryController.ListGlossaryByTerm(portalId, term);
                if (glossary.Count > 0)
                {
                    sb.Append(System.Web.HttpUtility.HtmlDecode(glossary[0].Definition));
                }
                else
                {
                    sb.Append("Term not found!");
                }
            }
            context.Response.Write(sb.ToString());
            context.Response.ContentType = "text/html";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}