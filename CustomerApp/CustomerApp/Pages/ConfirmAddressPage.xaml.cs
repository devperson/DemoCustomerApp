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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ShowCurrentLocation();
        }
        public async void ShowCurrentLocation()
        {
            //App.Locator.MainViewModel.User.UserAddress.Position = new Position(41.2610366907207, 69.1946012154222);
            //App.Locator.MainViewModel.User.UserAddress.AddressText = "Yakkabag Str. Ташкент Узбекистан";

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
                App.Locator.MainViewModel.User.Address = userAddress;       
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Error", "Error on getting current user location: " + ex.Message, "OK");

                return;
            }

            var address = userAddress.AddressText;
            if (address != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(userAddress.Position, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));
                map.Pins.Clear();
                map.Pins.Add(new Pin { Label = address, Address = address, Position = userAddress.Position });

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
                App.Locator.MainViewModel.UpdateUserLocation((res) =>
                {
                    if (res.Success)
                    {
                        var user = App.Locator.MainViewModel.User;
                        int id = user.Id;

                        CrossSettings.Current.AddOrUpdateValue<string>("AddressText" + id, user.Address.AddressText);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lat" + id, user.Address.Position.Latitude);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lon" + id, user.Address.Position.Longitude);

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
                var user = App.Locator.MainViewModel.User;
                user.Address = userAddress;

                int id = user.Id;
                App.Locator.MainViewModel.UpdateUserLocation((res) =>
                {
                    if (res.Success)
                    {
                        CrossSettings.Current.AddOrUpdateValue<string>("AddressText" + id, user.Address.AddressText);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lat" + id, user.Address.Position.Latitude);
                        CrossSettings.Current.AddOrUpdateValue<double>("Lon" + id, user.Address.Position.Longitude);
                        App.Current.MainPage = new MainPage();
                    }
                    else
                        this.DisplayAlert("Error", "Error on saving user address.", "Close");
                });           
            }
        }
    }
}
