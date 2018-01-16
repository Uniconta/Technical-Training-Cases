using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Uniconta.API.DebtorCreditor;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.Common;
using Uniconta.Common.User;
using Uniconta.DataModel;

namespace TableCreator.Core.Managers
{
    public static class UnicontaAPIManager
    {
        // Fields
        private static Guid accessGuid;
        private static UnicontaConnection unicontaConnection;
        private static Session unicontaSession;

        private static Company[] companies;
        private static Company currentCompany;

        public static void Initialize()
        {
            // Checks if the UnicontaAPIManager has already been initialized
            if (unicontaSession != null)
                throw new InvalidOperationException();

            // TODO: Initialize accessGuid
            // TODO: Initialize unicontaConnection
            // TODO: Initialize unicontaSession
        }

        #region Get / Set Methods
        public static User GetUser()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Login Methods
        public async static Task<ErrorCodes> Login(string username, string password)
        {
            if (accessGuid == Guid.Empty)
            {
                MessageBox.Show("ERROR: API Key not set");
                throw new AccessViolationException();
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return ErrorCodes.NoSucces;


            // TODO: Implement session LoginAsync
            throw new NotImplementedException();
        }

        public static void Logout()
        {
            // TODO: Implement session LogOut
            throw new NotImplementedException();
        }
        #endregion

        #region Company Methods
        public static async Task InitializeCompanies()
        {
            // Check if logged in
            if (await unicontaSession.IsLoggedIn() != ErrorCodes.Succes)
                throw new InvalidOperationException();

            // Getting companies
            // TODO: Implement session GetCompanies

            // Getting user default company
            // TODO: Implement default Company Selection
        }

        public static int GetCurrentCompanyId() { return currentCompany.CompanyId; }
        public static Company GetCurrentCompany() { return currentCompany; }

        public async static Task SetCurrentCompany(Company company) { await SetCurrentCompany(company.CompanyId); }
        public async static Task SetCurrentCompany(int companyId)
        {

            if (unicontaSession == null)
                throw new InvalidOperationException();

            // TODO: Implement session OpenCompany
            throw new NotImplementedException();
        }
        #endregion

        #region CrudAPI Method
        public static CrudAPI GetCrudAPI(Company company = null)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();
            
            // TODO: Implement CrudAPI
            throw new NotImplementedException();
        }
        #endregion

        #region InvoiceAPI Method
        public static InvoiceAPI GetInvoiceAPI(Company company = null)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();

            // TODO: Implement InvoiceAPI
            throw new NotImplementedException();
        }
        #endregion
    }
}