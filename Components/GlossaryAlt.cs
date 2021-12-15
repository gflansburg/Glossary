using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Gafware.Modules.Glossary.Components
{
    public class GlossaryAlt : ContentItem
    {
        /// <summary>
        /// Id of the alternate glossary term
        /// </summary>
        public int AltId { get; set; }
        /// <summary>
        /// Id of glossary term
        /// </summary>
        public int GlossaryId { get; set; }
        /// <summary>
        /// Term
        /// </summary>
        public string Term { get; set; }

        public override void Fill(IDataReader dr)
        {
            //base.Fill(dr);

            AltId = Null.SetNullInteger(dr["AltID"]);
            GlossaryId = Null.SetNullInteger(dr["GlossaryID"]);
            Term = Null.SetNullString(dr["AlternateTerm"]);
        }

        public override int KeyID
        {
            get
            {
                return AltId;
            }
            set
            {
                AltId = value;
            }
        }
    }
}