using System;
using System.Linq;
using System.Threading.Tasks;
using Uniconta.API.DebtorCreditor;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.Common;
using Uniconta.Common.User;
using Uniconta.DataModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Case9.Core.Managers
{
    public static class UnicontaAPIManager
    {
        // Fields
        private static string username;
        private static string password;

        private static Guid accessGuid;
        private static UnicontaConnection unicontaConnection;
        private static Session unicontaSession;

        private static Company[] companies;
        private static Company currentCompany;

        public static void Initialize()
        {
            // Checks if the UnicontaAPIManager has already been initialized
            if (unicontaSession != null)
                return;

            // TODO: Change API Key
            accessGuid = new Guid("00000000-0000-0000-0000-000000000000");
            unicontaConnection = new UnicontaConnection(APITarget.Live, true);
            unicontaSession = new Session(unicontaConnection);
        }

        #region Login Methods
        public async static Task<ErrorCodes> Login(string username, string password)
        {
            if (accessGuid == Guid.Empty)
                return ErrorCodes.KeyIsEmpty;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return ErrorCodes.NoSucces;

            // Saving username / password for later use
            UnicontaAPIManager.username = username;
            UnicontaAPIManager.password = password;

            return await unicontaSession.LoginAsync(username, password, LoginType.Android, accessGuid);
        }

        public static void Logout()
        {
            unicontaSession.LogOut();
        }

        public async static Task<bool> IsLoggedIn()
        {
            if (!unicontaSession.LoggedIn)
                return false;

            var errorCode = await unicontaSession.IsLoggedIn();
            if (errorCode == ErrorCodes.Succes)
                return true;
            else
                return false;
        }
        #endregion

        #region User Methods
        public static User GetCurrentUser() { return unicontaSession.User; }
        public static string GetCurrentUsername() { return unicontaSession.User._Name; }
        public static int GetCurrentUserId() { return unicontaSession.User.Uid; }
        #endregion

        #region Company Methods
        public static Company[] GetCompanies() { return companies; }

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

        #region CrudAPI Methods
        public static CrudAPI GetCrudAPI(Company company = null)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();

            return new CrudAPI(unicontaSession, company);
        }
        #endregion

        #region InvoiceAPI Methods
        public static InvoiceAPI GetInvoiceAPI(Company company = null)
        {
            if (unicontaSession == null)
                throw new InvalidOperationException();

            return new InvoiceAPI(unicontaSession, company);
        }
        #endregion
    }
}
