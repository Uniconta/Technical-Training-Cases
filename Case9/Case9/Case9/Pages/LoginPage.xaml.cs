using Case9.Core.Managers;
using Uniconta.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Case9.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();

            // Events
            this.LoginButton.Clicked += LoginButton_Clicked;
		}

        private async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            var loginErrorCode = await UnicontaAPIManager.Login(username, password);
            if(loginErrorCode != ErrorCodes.Succes)
            {
                MessageLabel.Text = $"Login failed {loginErrorCode.ToString()}";
                PasswordEntry.Text = string.Empty;
                return;
            }

            await UnicontaAPIManager.InitializeCompanies();
            await Navigation.PushAsync(new MainPage());
        }
    }
}