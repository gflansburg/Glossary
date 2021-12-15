using System;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using System.Linq;
using Gafware.Modules.Glossary.Data;

namespace Gafware.Modules.Glossary.Components
{
    public class GlossaryController
    {
        public const string CSS_TAG_INCLUDE_FORMAT = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
        public const string SCRIPT_TAG_INCLUDE_FORMAT = "<script language=\"javascript\" type=\"text/javascript\" src=\"{0}\"></script>";
        public const string USE_FRIENDLY_URL_KEY = "use.friendly.url";
        public const string CATEGORY_VISIBLE_KEY = "category.visible";
        public const string DEFAULT_CATEGORY_KEY = "default.category";
        public const string GLOSSARY_PAGE_KEY = "glossary.page";
        public const string STYLE_KEY = "tooltip.style";
        public const string ROUNDED_KEY = "tooltip.rounded";
        public const string SHADOWED_KEY = "tooltip.shadowed";
        public const string USE_IMAGE_KEY = "tooltip.use.image";
        public const string IMAGE_KEY = "tooltip.image";
        public const string SHOW_KEY = "tooltip.show";
        public const string HIDE_KEY = "tooltip.hide";
        public const string LINK_COLOR_KEY = "link.color";
        public const string HOVER_COLOR_KEY = "hover.color";
        public const string UNDERLINE_COLOR_KEY = "underline.color";
        public const string UNDERLINE_HOVER_COLOR_KEY = "underline.hover.color";
        public const string LINK_UNDERLINE_KEY = "link.underline";
        public const string LINK_BOLD_KEY = "link.bold";
        public const string UNDERLINE_WIDTH_KEY = "underline.width";
        public const string POSITION_MY_X_KEY = "tooltip.position.my.x";
        public const string POSITION_MY_Y_KEY = "tooltip.position.my.y";
        public const string POSITION_AT_X_KEY = "tooltip.position.at.x";
        public const string POSITION_AT_Y_KEY = "tooltip.position.at.y";

        #region "Public Methods"
        // Get a single Glossary item by primary key

        public static GlossaryInfo GetGlossary(long glossaryID)
        {
            return CBO.FillObject<GlossaryInfo>(DataProvider.Instance().GetGlossary(glossaryID));
        }

        //List all Glossary items
        public static List<GlossaryInfo> ListGlossary(int portalID)
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListGlossary(portalID));
        }

        //Find glossaries by term
        public static List<GlossaryInfo> ListGlossaryByTerm(int portalID, string term)
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListGlossaryByTerm(portalID, term));
        }

        //List glossaries by filter 
        public static List<GlossaryInfo> ListGlossaryByFilter(int portalID, string filter)
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListGlossaryByFilter(portalID, filter));
        }

        //List golssaries by filter and category
        public static List<GlossaryInfo> ListGlossaryByFilterAndCategory(int portalID, string filter, long glossaryCategoryDefinitionID)
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListGlossaryByFilterAndCategory(portalID, filter, glossaryCategoryDefinitionID));
        }

        //List glossaries by category
        public static List<GlossaryInfo> ListGlossaryByCategory(int portalID, long glossaryCategoryDefinitionID)
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListGlossaryByCategory(portalID, glossaryCategoryDefinitionID));
        }

        //List gossaries by letter
        public static List<GlossaryInfo> ListGlossaryByLetter(int portalID, string letter)
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListGlossaryByLetter(portalID, letter));
        }

        public static int SaveGlossary(GlossaryInfo objGlossary)
        {
            if (objGlossary.GlossaryId < 1)
            {
                objGlossary.GlossaryId = DataProvider.Instance().CreateGlossary(objGlossary);
            }
            else
            {
                DataProvider.Instance().UpdateGlossary(objGlossary);
            }
            return objGlossary.GlossaryId;
        }

        public static void DeleteGlossary(int glossaryId)
        {
            DataProvider.Instance().DeleteGlossary(glossaryId);
        }

        // Get a single GlossaryCategory item by primary key
        public static GlossaryCategory GetGlossaryCategory(long glossaryCategoryID)
        {
            return CBO.FillObject<GlossaryCategory>(DataProvider.Instance().GetGlossaryCategory(glossaryCategoryID));
        }

        public static List<GlossaryCategory> ListGlossaryCategories()
        {
            return CBO.FillCollection<GlossaryCategory>(DataProvider.Instance().ListGlossaryCategories());
        }

        public static List<GlossaryCategory> GetGlossaryCategoryByGlossary(long glossaryID)
        {
            return CBO.FillCollection<GlossaryCategory>(DataProvider.Instance().GetGlossaryCategoryByGlossary(glossaryID));
        }
        public static List<GlossaryCategory> GetGlossaryCategoryByGlossaryCategoryDefinition(long glossaryCategoryDefinitionID)
        {
            return CBO.FillCollection<GlossaryCategory>(DataProvider.Instance().GetGlossaryCategoryByGlossaryCategoryDefinition(glossaryCategoryDefinitionID));
        }

        public static int SaveGlossaryCategory(GlossaryCategory objGlossaryCategory)
        {
            if (objGlossaryCategory.GlossaryCategoryId < 1)
            {
                objGlossaryCategory.GlossaryCategoryId = DataProvider.Instance().CreateGlossaryCategory(objGlossaryCategory);
            }
            else
            {
                DataProvider.Instance().UpdateGlossaryCategory(objGlossaryCategory);
            }
            return objGlossaryCategory.GlossaryCategoryId;
        }

        public static void DeleteGlossaryCategory(long glossaryCategoryID)
        {
            DataProvider.Instance().DeleteGlossaryCategory(glossaryCategoryID);
        }

        public static void DeleteGlossaryCategoryByGlossary(long glossaryID)
        {
            DataProvider.Instance().DeleteGlossaryCategoryByGlossary(glossaryID);
        }

        // Get a single GlossaryCategoryDefinition item by primary key
        public static GlossaryCategoryDefinition GetGlossaryCategoryDefinition(long glossaryCategoryDefinitionID)
        {
            return CBO.FillObject<GlossaryCategoryDefinition>(DataProvider.Instance().GetGlossaryCategoryDefinition(glossaryCategoryDefinitionID));
        }

        public static List<GlossaryCategoryDefinition> ListGlossaryCategoryDefinitions(int portalId)
        {
            return CBO.FillCollection<GlossaryCategoryDefinition>(DataProvider.Instance().ListGlossaryCategoryDefinitions(portalId));
        }

        public static List<GlossaryCategoryDefinition> ListGlossaryCategoryDefinitionsByGlossary(int glossaryId)
        {
            return CBO.FillCollection<GlossaryCategoryDefinition>(DataProvider.Instance().ListGlossaryCategoryDefinitionsByGlossary(glossaryId));
        }

        public static int SaveGlossaryCategoryDefinition(GlossaryCategoryDefinition objGlossaryCategoryDefinition)
        {
            if (objGlossaryCategoryDefinition.GlossaryCategoryDefinitionId < 1)
            {
                objGlossaryCategoryDefinition.GlossaryCategoryDefinitionId = DataProvider.Instance().CreateGlossaryCategoryDefinition(objGlossaryCategoryDefinition);
            }
            else
            {
                DataProvider.Instance().UpdateGlossaryCategoryDefinition(objGlossaryCategoryDefinition);
            }
            return objGlossaryCategoryDefinition.GlossaryCategoryDefinitionId;
        }

        public static void DeleteGlossaryCategoryDefinition(int glossaryCategoryDefinitionId)
        {
            DataProvider.Instance().DeleteGlossaryCategoryDefinition(glossaryCategoryDefinitionId);
        }

        public static List<GlossaryAlt> ListGlossaryAltTerms(int glossaryId)
        {
            return CBO.FillCollection<GlossaryAlt>(DataProvider.Instance().ListGlossaryAltTerms(glossaryId));
        }

        public static void DeleteGlossaryAlt(int glossaryAltId)
        {
            DataProvider.Instance().DeleteGlossaryAltTerm(glossaryAltId);
        }

        public static int SaveGlossaryAlt(GlossaryAlt objGlossaryAlt)
        {
            if (objGlossaryAlt.AltId < 1)
            {
                objGlossaryAlt.AltId = DataProvider.Instance().CreateGlossaryAltTerm(objGlossaryAlt);
            }
            else
            {
                DataProvider.Instance().UpdateGlossaryAltTerm(objGlossaryAlt);
            }
            return objGlossaryAlt.AltId;
        }

        public static List<GlossaryInfo> ListAllGlossary()
        {
            return CBO.FillCollection<GlossaryInfo>(DataProvider.Instance().ListAllGlossary());
        }

        public static List<GlossaryPage> ListGlossaryPages(int glossaryId)
        {
            return CBO.FillCollection<GlossaryPage>(DataProvider.Instance().ListGlossaryPages(glossaryId));
        }

        public static List<GlossaryPage> ListGlossaryPagesByTerm(string term)
        {
            return CBO.FillCollection<GlossaryPage>(DataProvider.Instance().ListGlossaryPagesByTerm(term));
        }

        public static List<GlossaryPage> ListGlossaryPagesByTab(int tabId)
        {
            return CBO.FillCollection<GlossaryPage>(DataProvider.Instance().ListGlossaryPagesByTab(tabId));
        }

        public static List<GlossaryPage> ListGlossaryPagesByPortal(int portalId)
        {
            return CBO.FillCollection<GlossaryPage>(DataProvider.Instance().ListGlossaryPagesByPortal(portalId));
        }

        public static void DeleteGlossaryPage(int pageId)
        {
            DataProvider.Instance().DeleteGlossaryPage(pageId);
        }

        public static int SaveGlossaryPage(GlossaryPage objGlossaryPage)
        {
            if (objGlossaryPage.PageId < 1)
            {
                objGlossaryPage.PageId = DataProvider.Instance().CreateGlossaryPage(objGlossaryPage);
            }
            else
            {
                DataProvider.Instance().UpdateGlossaryPage(objGlossaryPage);
            }
            return objGlossaryPage.PageId;
        }
        #endregion
    }
}