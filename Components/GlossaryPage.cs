using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Gafware.Modules.Glossary.Components
{
    public class GlossaryPage : ContentItem
    {
        /// <summary>
        /// Id of the glossary term page
        /// </summary>
        public int PageId { get; set; }
        /// <summary>
        /// Id of glossary term
        /// </summary>
        public int GlossaryId { get; set; }
        /// <summary>
        /// Tab id of tab
        /// </summary>
        public int TabId { get; set; }
        /// <summary>
        /// Term text
        /// </summary>
        public string Term { get; set; }

        public override void Fill(IDataReader dr)
        {
            //base.Fill(dr);

            PageId = Null.SetNullInteger(dr["PageID"]);
            GlossaryId = Null.SetNullInteger(dr["GlossaryID"]);
            TabId = Null.SetNullInteger(dr["TabID"]);
            Term = Null.SetNullString(dr["Term"]);
        }

        public override int KeyID
        {
            get
            {
                return PageId;
            }
            set
            {
                PageId = value;
            }
        }
    }
}