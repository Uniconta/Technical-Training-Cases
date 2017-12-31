using Case9.Core.Managers;
using Case9.Pages;

using Xamarin.Forms;

namespace Case9
{
    public partial class App : Application
	{
		public App()
		{
            UnicontaAPIManager.Initialize();

            InitializeComponent();
            Initialize();
        }

        private async void Initialize()
        {
            if (await UnicontaAPIManager.IsLoggedIn())
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
