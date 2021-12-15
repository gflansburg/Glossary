using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace Gafware.Modules.Glossary.Components
{
    public class GlossaryCategoryDefinition : ContentItem
    {
        /// <summary>
        /// Id of Glossary Category Defintion
        /// </summary>
        public int GlossaryCategoryDefinitionId { get; set; }
        /// <summary>
        /// Portal Id of Glossary Category Defintion
        /// </summary>
        public int PortalId { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        public string CategoryName { get; set; }

        public override void Fill(IDataReader dr)
        {
            //base.Fill(dr);

            GlossaryCategoryDefinitionId = Null.SetNullInteger(dr["GlossaryCategoryDefinitionID"]);
            PortalId = Null.SetNullInteger(dr["PortalID"]);
            CategoryName = Null.SetNullString(dr["CategoryName"]);
        }

        public override int KeyID
        {
            get
            {
                return GlossaryCategoryDefinitionId;
            }
            set
            {
                GlossaryCategoryDefinitionId = value;
            }
        }
    }
}