/*
' Copyright (c) 2021 Gafware
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
using System.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;
using Gafware.Modules.Glossary.Components;

namespace Gafware.Modules.Glossary.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// 
    /// This concreted data provider class provides the implementation of the abstract methods 
    /// from data dataprovider.cs
    /// 
    /// In most cases you will only modify the Public methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "Gafware_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        // used to prefect your database objects (stored procedures, tables, views, etc)
        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        public override IDataReader GetGlossary(long glossaryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryGet", glossaryId);
        }

        public override IDataReader ListGlossary(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryList", portalId);
        }

        public override IDataReader ListGlossaryByTerm(int portalId, string term)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryListByTerm", portalId, term);
        }

        public override IDataReader ListGlossaryByFilter(int portalId, string filter)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryListByFilter", portalId, filter);
        }

        public override IDataReader ListGlossaryByFilterAndCategory(int portalId, string filter, long glossaryCategoryDefinitionId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryListByFilterAndCategory", glossaryCategoryDefinitionId, portalId, filter);
        }

        public override IDataReader ListGlossaryByCategory(int portalId, long glossaryCategoryDefinitionId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryListByCategory", glossaryCategoryDefinitionId, portalId);
        }

        public override IDataReader ListGlossaryByLetter(int portalId, string letter)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryListByLetter", portalId, letter);
        }

        public override int CreateGlossary(GlossaryInfo objGlossary)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "GlossaryCreate", objGlossary.Term, objGlossary.Definition, objGlossary.PortalId));
        }

        public override void UpdateGlossary(GlossaryInfo objGlossary)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryUpdate", objGlossary.GlossaryId, objGlossary.Term, objGlossary.Definition, objGlossary.PortalId);
        }

        public override void DeleteGlossary(long glossaryId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryDelete", glossaryId);
        }

        public override IDataReader GetGlossaryCategory(long glossaryCategoryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryCategoryGet", glossaryCategoryId);
        }

        public override IDataReader ListGlossaryCategories()
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, NamePrefix + "GlossaryCategoryList");
        }

        public override IDataReader GetGlossaryCategoryByGlossary(long glossaryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryCategoryGetByGafware_Glossary", glossaryId);
        }

        public override IDataReader GetGlossaryCategoryByGlossaryCategoryDefinition(long glossaryCategoryDefinitionId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryCategoryGetByGafware_GlossaryCategoryDefinition", glossaryCategoryDefinitionId);
        }

        public override int CreateGlossaryCategory(GlossaryCategory objGlossaryCategory)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "GlossaryCategoryCreate", objGlossaryCategory.GlossaryId, objGlossaryCategory.GlossaryCategoryDefinitionId));
        }

        public override void UpdateGlossaryCategory(GlossaryCategory objGlossaryCategory)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryCategoryUpdate", objGlossaryCategory.GlossaryCategoryId, objGlossaryCategory.GlossaryId, objGlossaryCategory.GlossaryCategoryDefinitionId);
        }

        public override void DeleteGlossaryCategory(long glossaryCategoryId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryCategoryDelete", glossaryCategoryId);
        }

        public override void DeleteGlossaryCategoryByGlossary(long glossaryId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryCategoryDeleteByGlossary", glossaryId);
        }

        public override IDataReader GetGlossaryCategoryDefinition(long glossaryCategoryDefinitionId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryCategoryDefinitionGet", glossaryCategoryDefinitionId);
        }

        public override IDataReader ListGlossaryCategoryDefinitions(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryCategoryDefinitionList", portalId);
        }

        public override IDataReader ListGlossaryCategoryDefinitionsByGlossary(int glossaryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryCategoryDefinitionListByGlossary", glossaryId);
        }

        public override int CreateGlossaryCategoryDefinition(GlossaryCategoryDefinition objGlossaryCategoryDefinition)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "GlossaryCategoryDefinitionCreate", objGlossaryCategoryDefinition.PortalId, objGlossaryCategoryDefinition.CategoryName));
        }

        public override void UpdateGlossaryCategoryDefinition(GlossaryCategoryDefinition objGlossaryCategoryDefinition)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryCategoryDefinitionUpdate", objGlossaryCategoryDefinition.GlossaryCategoryDefinitionId, objGlossaryCategoryDefinition.PortalId, objGlossaryCategoryDefinition.CategoryName);
        }

        public override void DeleteGlossaryCategoryDefinition(long glossaryCategoryDefinitionId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryCategoryDefinitionDelete", glossaryCategoryDefinitionId);
        }

        public override IDataReader ListGlossaryAltTerms(int glossaryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryAltTermList", glossaryId);
        }

        public override void DeleteGlossaryAltTerm(long altId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryAltTermDelete", altId);
        }

        public override void UpdateGlossaryAltTerm(GlossaryAlt objGlossaryAlt)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryAltTermUpdate", objGlossaryAlt.AltId, objGlossaryAlt.GlossaryId, objGlossaryAlt.Term);
        }

        public override int CreateGlossaryAltTerm(GlossaryAlt objGlossaryAlt)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "GlossaryAltTermCreate", objGlossaryAlt.GlossaryId, objGlossaryAlt.Term));
        }

        public override IDataReader ListAllGlossary()
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryList", -1);
        }

        public override IDataReader ListGlossaryPages(int glossaryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryPageList", glossaryId);
        }

        public override IDataReader ListGlossaryPagesByTerm(string term)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryPageListByTerm", term);
        }

        public override IDataReader ListGlossaryPagesByTab(int tabId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryPageListByTab", tabId);
        }

        public override IDataReader ListGlossaryPagesByPortal(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GlossaryPageListByPortal", portalId);
        }

        public override void DeleteGlossaryPage(long pageId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryPageDelete", pageId);
        }

        public override void UpdateGlossaryPage(GlossaryPage objGlossaryPage)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "GlossaryPageUpdate", objGlossaryPage.PageId, objGlossaryPage.GlossaryId, objGlossaryPage.TabId);
        }

        public override int CreateGlossaryPage(GlossaryPage objGlossaryPage)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "GlossaryPageCreate", objGlossaryPage.GlossaryId, objGlossaryPage.TabId));
        }

        #endregion

    }

}