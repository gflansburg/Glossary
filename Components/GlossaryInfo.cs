using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;
using Gafware.Modules.Glossary.Data;

namespace Gafware.Modules.Glossary.Components
{
    public class GlossaryInfo : ContentItem
    {
        /// <summary>
        /// Id of glossary term
        /// </summary>
        public int GlossaryId { get; set; }
        /// <summary>
        /// Term
        /// </summary>
        public string Term { get; set; }
        /// <summary>
        /// Definition
        /// </summary>
        public string Definition { get; set; }
        /// <summary>
        /// Portal Id
        /// </summary>
        public int PortalId { get; set; }
        /// <summary>
        /// Alternate terms
        /// </summary>
        public List<GlossaryAlt> AltTerms { get; set; }
        /// <summary>
        /// Pages
        /// </summary>
        public List<GlossaryPage> Pages { get; set; }
        /// <summary>
        /// Created on date
        /// </summary>
        public new DateTime LastModifiedOnDate { get; set; }

        public override void Fill(IDataReader dr)
        {
            //base.Fill(dr);

            GlossaryId = Null.SetNullInteger(dr["GlossaryID"]);
            Term = Null.SetNullString(dr["Term"]);
            Definition = Null.SetNullString(dr["Definition"]);
            PortalId = Null.SetNullInteger(dr["PortalId"]);
            LastModifiedOnDate = Null.SetNullDateTime(dr["DateTimeStamp"]);
            AltTerms = GlossaryController.ListGlossaryAltTerms(GlossaryId);
            Pages = GlossaryController.ListGlossaryPages(GlossaryId);
        }

        public override int KeyID
        {
            get
            {
                return GlossaryId;
            }
            set
            {
                GlossaryId = value;
            }
        }
    }
}