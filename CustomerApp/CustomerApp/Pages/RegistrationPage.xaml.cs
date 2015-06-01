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
            
            this.BindingContext = App.Locator.MainViewModel.User;

            genderPicker.Items.Add("Male");
            genderPicker.Items.Add("Female");
        }

        private void genderPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            var user = App.Locator.MainViewModel.User;
            user.Gender = genderPicker.Items[genderPicker.SelectedIndex];
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            errorMsg.IsVisible = false;
            var user = App.Locator.MainViewModel.User;
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                errorMsg.IsVisible = true;
                lblErr.Text = "Fill all fields.";
                return;
            }

            App.Locator.MainViewModel.WebService.RegisterUser(this.BindingContext as User, (res) =>
            {
                if (res.Success)
                {
                    App.Locator.MainViewModel.User.Id = res.UserId;

                    App.Locator.MainViewModel.OnUserLogedIn();
                    this.Navigation.PushAsync(new ConfirmAddressPage());
                }
                else
                {
                    errorMsg.IsVisible = true;
                    lblErr.Text = res.Error;
                }
            });
        }
    }
}
