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

            // TODO: Change API Key
            accessGuid = new Guid("00000000-0000-0000-0000-000000000000");
            unicontaConnection = new UnicontaConnection(APITarget.Live);
            unicontaSession = new Session(unicontaConnection);
        }

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

            return await unicontaSession.LoginAsync(username, password, LoginType.API, accessGuid);
        }

        public static void Logout()
        {
            unicontaSession.LogOut();
        }
        #endregion

        #region Company Methods
        public static async Task InitializeCompanies()
        {
            // Check if logged in
            if (await unicontaSession.IsLoggedIn() != ErrorCodes.Succes)
                throw new InvalidOperationException();

            // Getting companies
            companies = await unicontaSession.GetCompanies();

            // Getting user default company
            if (unicontaSession.User._DefaultCompany != 0)
                await SetCurrentCompany(unicontaSession.User._DefaultCompany);
            else
            {
                var company = companies.FirstOrDefault();
                if (company == null)
                    throw new InvalidOperationException();

                await SetCurrentCompany(company);
            }
        }

        public static int GetCurrentCompanyId() { return currentCompany.CompanyId; }
        public static Company GetCurrentCompany() { return currentCompany; }

        public async static Task SetCurrentCompany(Company company) { await SetCurrentCompany(company.CompanyId); }
        public async static Task SetCurrentCompany(int companyId)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();

            currentCompany = await unicontaSession.OpenCompany(companyId, true);
        }
        #endregion

        #region CrudAPI Method
        public static CrudAPI GetCrudAPI(Company company = null)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();

            return new CrudAPI(unicontaSession, company);
        }
        #endregion

        #region InvoiceAPI Method
        public static InvoiceAPI GetInvoiceAPI(Company company = null)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();

            return new InvoiceAPI(unicontaSession, company);
        }
        #endregion
    }
}