using CustomerApp.Controls.Models;
using Geolocator.Plugin;
using Refractored.Xam.Settings;
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


            ShowCurrentLocation();
        }

        public void ShowCurrentLocation()
        {
            //App.Locator.MainViewModel.User.UserAddress.Position = new Position(41.2610366907207, 69.1946012154222);
            //App.Locator.MainViewModel.User.UserAddress.AddressText = "Yakkabag Str. Ташкент Узбекистан";

            var pos = App.Locator.MainViewModel.User.UserAddress.Position;
            var address = App.Locator.MainViewModel.User.UserAddress.AddressText;
            if (address != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));
                map.Pins.Clear();
                map.Pins.Add(new Pin { Label = address, Address = address, Position = pos });

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.DisplayAlert(address);
                    });

                    return false;
                });
            }
        }

        private async void DisplayAlert(string address)
        {
            var result = await this.DisplayAlert("Delivery Address", address, "Confirm", "Cancel");
            if (result)
            {
                var pos = App.Locator.MainViewModel.User.UserAddress.Position;
                App.Locator.MainViewModel.UpdateUserLocation((res) =>
                {
                    if (res.Success)
                    {
                        CrossSettings.Current.AddOrUpdateValue<string>("AddressText", App.Locator.MainViewModel.User.UserAddress.AddressText);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lat", App.Locator.MainViewModel.User.UserAddress.Position.Latitude);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lon", App.Locator.MainViewModel.User.UserAddress.Position.Longitude);

                        App.Current.MainPage = new MainPage();
                    }
                    else
                        this.DisplayAlert("Error", "Error occured while registering user location.", "Close");
                });                
            }
        }       

        public async void SearchAddress_Clicked(object sender, EventArgs e)
        {
            var geo = new Geocoder();
            var positions = await geo.GetPositionsForAddressAsync(searchBar.Text);
            if (positions.Any())
            {
                var position = positions.First();
                map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));
                ConfirmLocation(position);
            }
            else
                this.DisplayAlert("Error", "Couldn't get any location for '" + searchBar.Text + "'", "Close");
        }

        private void map_Tap(object sender, Controls.TapEventArgs e)
        {
            this.ConfirmLocation(e.Position);
        }

        private async void ConfirmLocation(Position pos)
        {
            var geo = new Geocoder();
            var addresses = await geo.GetAddressesForPositionAsync(pos);
            var address = addresses.First();

            var userAddress = new Address();
            userAddress.AddressText = address;
            userAddress.Position = pos;

            map.Pins.Clear();
            map.Pins.Add(new Pin { Label = address, Address = address, Position = pos });

            var result = await this.DisplayAlert("Delivery Address", address, "Confirm", "Cancel");
            if (result)
            {
                App.Locator.MainViewModel.User.UserAddress = userAddress;

                int id = App.Locator.MainViewModel.User.Id;
                App.Locator.MainViewModel.UpdateUserLocation((res) =>
                {
                    if (res.Success)
                    {
                        CrossSettings.Current.AddOrUpdateValue<string>("AddressText", App.Locator.MainViewModel.User.UserAddress.AddressText);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lat", App.Locator.MainViewModel.User.UserAddress.Position.Latitude);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lon", App.Locator.MainViewModel.User.UserAddress.Position.Longitude);
                        App.Current.MainPage = new MainPage();
                    }
                    else
                        this.DisplayAlert("Error", "Error on saving user address.", "Close");
                });           
            }
        }
    }
}
