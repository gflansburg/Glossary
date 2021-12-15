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

using System.Data;
using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Gafware.Modules.Glossary.Components;

namespace Gafware.Modules.Glossary.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// An abstract class for the data access layer
    /// 
    /// The abstract data provider provides the methods that a control data provider (sqldataprovider)
    /// must implement. You'll find two commented out examples in the Abstract methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {

        #region Shared/Static Methods

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                const string assembly = "Gafware.Modules.Glossary.Data.SqlDataprovider,Gafware.Glossary";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString.ToString();
            newConnection.Open();
            return newConnection;
        }

        #endregion

        #region Abstract methods

        public abstract IDataReader GetGlossary(long glossaryId);

        public abstract IDataReader ListGlossary(int portalId);

        public abstract IDataReader ListGlossaryByTerm(int portalId, string term);

        public abstract IDataReader ListGlossaryByFilter(int portalId, string filter);

        public abstract IDataReader ListGlossaryByFilterAndCategory(int portalId, string filter, long glossaryCategoryDefinitionId);

        public abstract IDataReader ListGlossaryByCategory(int portalId, long glossaryCategoryDefinitionId);

        public abstract IDataReader ListGlossaryByLetter(int portalId, string letter);

        public abstract int CreateGlossary(GlossaryInfo objGlossary);

        public abstract void UpdateGlossary(GlossaryInfo objGlossary);

        public abstract void DeleteGlossary(long glossaryId);

        public abstract IDataReader GetGlossaryCategory(long glossaryCategoryId);

        public abstract IDataReader ListGlossaryCategories();

        public abstract IDataReader GetGlossaryCategoryByGlossary(long glossaryId);

        public abstract IDataReader GetGlossaryCategoryByGlossaryCategoryDefinition(long glossaryCategoryDefinitionId);

        public abstract int CreateGlossaryCategory(GlossaryCategory objGlossaryCategory);

        public abstract void UpdateGlossaryCategory(GlossaryCategory objGlossaryCategory);

        public abstract void DeleteGlossaryCategory(long glossaryCategoryId);

        public abstract void DeleteGlossaryCategoryByGlossary(long glossaryId);

        public abstract IDataReader GetGlossaryCategoryDefinition(long glossaryCategoryDefinitionId);

        public abstract IDataReader ListGlossaryCategoryDefinitions(int portalId);

        public abstract IDataReader ListGlossaryCategoryDefinitionsByGlossary(int glossaryId);

        public abstract int CreateGlossaryCategoryDefinition(GlossaryCategoryDefinition objGlossaryCategoryDefinition);

        public abstract void UpdateGlossaryCategoryDefinition(GlossaryCategoryDefinition objGlossaryCategoryDefinition);

        public abstract void DeleteGlossaryCategoryDefinition(long glossaryCategoryDefinitionId);

        public abstract IDataReader ListGlossaryAltTerms(int glossaryId);

        public abstract int CreateGlossaryAltTerm(GlossaryAlt objGlossaryAlt);

        public abstract void UpdateGlossaryAltTerm(GlossaryAlt objGlossaryAlt);

        public abstract void DeleteGlossaryAltTerm(long glossaryAltId);

        public abstract IDataReader ListAllGlossary();

        public abstract IDataReader ListGlossaryPages(int glossaryId);

        public abstract IDataReader ListGlossaryPagesByTerm(string term);

        public abstract IDataReader ListGlossaryPagesByTab(int tabId);

        public abstract IDataReader ListGlossaryPagesByPortal(int portalId);

        public abstract int CreateGlossaryPage(GlossaryPage objGlossaryPage);

        public abstract void UpdateGlossaryPage(GlossaryPage objGlossaryPage);

        public abstract void DeleteGlossaryPage(long pageId);

        #endregion

    }

}