using CustomerApp.Controls.Models;
using Geolocator.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomerApp
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();

            this.GetPosition();
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new LoginPage());            
        }

        private void btnReg_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new RegistrationPage());
        }

        private async void GetPosition()
        {            
            var locator = CrossGeolocator.Current;            
            locator.DesiredAccuracy = 20;
            try
            {
                var geoLocation = await locator.GetPositionAsync();

                var pos = new Position(geoLocation.Latitude, geoLocation.Longitude);

                var geo = new Geocoder();
                var addresses = await geo.GetAddressesForPositionAsync(pos);
                var addr = addresses.First();

                var userAddress = new Address();
                userAddress.AddressText = addr;
                userAddress.Position = pos;
                App.Locator.MainViewModel.User.UserAddress = userAddress;

                Debug.WriteLine("User Location resolved!");
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Error", "Error on getting current user location: " + ex.Message, "OK");
            }
        }
    }
}
