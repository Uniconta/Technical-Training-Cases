using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Uniconta.API.Service;
using Uniconta.API.System;
using Uniconta.DataModel;

namespace PluginEmulator
{
    public static class DemoInitializer
    {
        /// <summary>
        /// Proeprty for Current Company
        /// </summary>
        public static Company CurrentCompany;

        /// <summary>
        /// Property for Containing Companies List 
        /// </summary>
        public static Company[] Companies;

        /// <summary>
        /// Property for Session Variable
        /// </summary>
        public static Session CurrentSession;

        /// <summary>
        /// Property for Getting and setting the UserName
        /// </summary>
        public static string UserName;

        /// <summary>
        /// Property for getting and setting the Password
        /// </summary>
        public static string Password;

        /// <summary>
        /// Property to save User Information
        /// </summary>
        public static bool IsUserPersist;
        
        /// <summary>
        /// Readonly Property to Get base API instance
        /// </summary>
        public static CrudAPI GetBaseAPI
        {
            get { return new CrudAPI(CurrentSession, CurrentCompany); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async public static Task SetCompany(int companyId)
        {
            CurrentCompany = CurrentSession.GetOpenCompany(companyId);
            if (CurrentCompany != null)
                CurrentSession.DefaultCompany = CurrentCompany;
            else
                CurrentCompany = await CurrentSession.OpenCompany(companyId, true);
        }

        /// <summary>
        /// Method to Get the Current Session
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Session GetSession() { return CurrentSession; }

        /// <summary>
        /// Method to Set all the Companies for the Loged In User
        /// </summary>
        /// <returns></returns>
        async public static Task SetupCompanies()
        {
            if (CurrentSession != null)
                Companies = await CurrentSession.GetCompanies();
        }

        /// <summary>
        /// Method which Initializes the Session
        /// </summary>
        /// <returns></returns>
        public static Session InitUniconta()
        {
            if (CurrentSession == null)
            {
                var corasauConnection = new UnicontaConnection(APITarget.Live);
                CurrentSession = new Session(corasauConnection);
            }
            return CurrentSession;
        }
    }
}
