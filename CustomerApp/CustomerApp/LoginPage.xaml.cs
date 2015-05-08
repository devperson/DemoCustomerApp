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

            App.User = new User();
            this.BindingContext = App.User;
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
