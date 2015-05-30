using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            
            this.BindingContext = App.Locator.MainViewModel.User;
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            errorMsg.IsVisible = false;
            var user = App.Locator.MainViewModel.User;

            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                errorMsg.IsVisible = true;
                return;
            }

            App.Locator.MainViewModel.WebService.Login(new { username = user.UserName, password = user.Password }, (res) =>
            {
                if (res.Success)
                {
                    user.Id = res.UserId;

                    App.Locator.MainViewModel.OnUserLogedIn();
                    App.Current.MainPage = new MainPage();
                }
                else
                    errorMsg.IsVisible = true;
            }); 
        }
    }
}
