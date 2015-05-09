using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();

            App.Locator.MainViewModel.User = new User();
            this.BindingContext = App.Locator.MainViewModel.User;
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.Navigation.PushAsync(new ConfirmAddressPage());
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
