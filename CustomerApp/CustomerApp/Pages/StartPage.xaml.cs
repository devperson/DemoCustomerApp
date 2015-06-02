using CustomerApp.Controls.Models;
using Geolocator.Plugin;
using Refractored.Xam.Settings;
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
            var addressText = CrossSettings.Current.GetValueOrDefault<string>("AddressText", null);

            if (!string.IsNullOrEmpty(addressText)) //get location from local cache
            {
                App.Locator.MainViewModel.User.UserAddress.AddressText = addressText;
                App.Locator.MainViewModel.User.UserAddress.Position = new Position(CrossSettings.Current.GetValueOrDefault<double>("Lat", 0), CrossSettings.Current.GetValueOrDefault<double>("Lon", 0));
            }
            else
            {
                var userAddress = new Address();
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 20;
                try
                {
                    var geoLocation = await locator.GetPositionAsync();

                    var pos = new Position(geoLocation.Latitude, geoLocation.Longitude);

                    var geo = new Geocoder();
                    var addresses = await geo.GetAddressesForPositionAsync(pos);
                    var addr = addresses.First();

                    userAddress.AddressText = addr;
                    userAddress.Position = pos;
                    App.Locator.MainViewModel.User.UserAddress = userAddress;

                    if (this.Navigation.NavigationStack.Last() is ConfirmAddressPage)
                    {
                        (this.Navigation.NavigationStack.Last() as ConfirmAddressPage).ShowCurrentLocation();
                    }

                    Debug.WriteLine("User Location resolved!");
                }
                catch (Exception ex)
                {
                    this.DisplayAlert("Error", "Error on getting current user location: " + ex.Message, "OK");
                }
            }
        }
    }
}
