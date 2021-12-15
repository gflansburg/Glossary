using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Gafware.Modules.Glossary.Components
{
    public class GlossaryCategory : ContentItem
    {
        /// <summary>
        /// Id of Glossary Category
        /// </summary>
        public int GlossaryCategoryId { get; set; }
        /// <summary>
        /// Id of Glossary
        /// </summary>
        public int GlossaryId { get; set; }
        /// <summary>
        /// Id of Glossary Category Definition
        /// </summary>
        public int GlossaryCategoryDefinitionId { get; set; }

        public override void Fill(IDataReader dr)
        {
            //base.Fill(dr);

            GlossaryCategoryId = Null.SetNullInteger(dr["GlossaryCategoryID"]);
            GlossaryId = Null.SetNullInteger(dr["GlossaryID"]);
            GlossaryCategoryDefinitionId = Null.SetNullInteger(dr["GlossaryCategoryDefinitionID"]);
        }

        public override int KeyID
        {
            get
            {
                return GlossaryCategoryId;
            }
            set
            {
                GlossaryCategoryId = value;
            }
        }
    }
}