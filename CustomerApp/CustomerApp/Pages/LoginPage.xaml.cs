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
            
            this.BindingContext = App.Locator.MainViewModel;

            App.Locator.MainViewModel.ShowAlert = this.DisplayAlert;
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            errorMsg.IsVisible = false;
            var user = App.Locator.MainViewModel.User;

            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                errorMsg.IsVisible = true;
                lbl.Text = "Please fill all fields.";
                return;
            }

            App.Locator.MainViewModel.LoadingCount++;
            App.Locator.MainViewModel.WebService.Login(new { username = user.UserName, password = user.Password }, (res) =>
            {
                App.Locator.MainViewModel.LoadingCount--;
                if (res.Success)
                {
                    user.Id = res.UserId;

                    App.Locator.MainViewModel.OnUserLogedIn();
                    App.Current.MainPage = new MainPage();
                }
                else
                {
                    lbl.Text = res.Error;
                    errorMsg.IsVisible = true;
                }
            }); 
        }
    }
}
