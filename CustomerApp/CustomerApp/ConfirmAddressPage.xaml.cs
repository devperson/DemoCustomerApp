using CustomerApp.Controls.Models;
using Geolocator.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomerApp
{
    public partial class ConfirmAddressPage : ContentPage
    {
        //private Position currentLocation;
        public ConfirmAddressPage()
        {
            InitializeComponent();

            map.IsShowingUser = true;
            map.Tap += map_Tap;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.GetPosition();

                return false;
            });
        }

        bool gotPosision = false;
        private async void GetPosition()
        {
            if (gotPosision)
                return;

            gotPosision = true;
            var locator = CrossGeolocator.Current;
            locator.PositionError += locator_PositionError;
            locator.DesiredAccuracy = 20;
            try
            {
                var geoLocation = await locator.GetPositionAsync();

                var pos = new Position(geoLocation.Latitude, geoLocation.Longitude);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));


                var geo = new Geocoder();
                var addresses = await geo.GetAddressesForPositionAsync(pos);
                var addr = addresses.First();

                var userAddress = new Address();
                userAddress.AddressText = addr;
                userAddress.Position = pos;

                map.Pins.Clear();
                map.Pins.Add(new Pin { Label = "Current Location", Address = userAddress.AddressText, Position = pos });

                var result = await this.DisplayAlert("Delivery Address", userAddress.AddressText, "Confirm", "Cancel");
                if (result)
                {
                    App.Locator.MainViewModel.User.UserAddress = userAddress;
                    App.Current.MainPage = new MainPage();
                }
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Error", "Error on getting current user location: " + ex.Message, "OK");
            }
        }

        private void locator_PositionError(object sender, Geolocator.Plugin.Abstractions.PositionErrorEventArgs e)
        {
            this.DisplayAlert("Error", "Error on getting current user location: " + e.Error, "OK");
        }

        public async void SearchAddress_Clicked(object sender, EventArgs e)
        {
            var geo = new Geocoder();
            var positions = await geo.GetPositionsForAddressAsync(searchBar.Text);
            var position = positions.First();
            
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));

            ConfirmLocation(position);
        }

        private void map_Tap(object sender, Controls.TapEventArgs e)
        {
            this.ConfirmLocation(e.Position);
        }

        private async void ConfirmLocation(Position pos)
        {
            var geo = new Geocoder();
            var addresses = await geo.GetAddressesForPositionAsync(pos);
            var addr = addresses.First();

            var userAddress = new Address();
            userAddress.AddressText = addr;
            userAddress.Position = pos;

            map.Pins.Clear();
            map.Pins.Add(new Pin { Label = userAddress.AddressText, Address = userAddress.AddressText, Position = pos });

            var result = await this.DisplayAlert("Delivery Address", userAddress.AddressText, "Confirm", "Cancel");
            if (result)
            {
                App.Locator.MainViewModel.User.UserAddress = userAddress;
                App.Current.MainPage = new MainPage();
            }
        }
    }
}
