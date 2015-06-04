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


            this.BindingContext = App.Locator.MainViewModel;
            map.IsShowingUser = true;
            map.Tap += map_Tap;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ShowCurrentLocation();
        }
        public void ShowCurrentLocation()
        {            
            var user = App.Locator.MainViewModel.User;
            var address = user.Address.AddressText;
            if (address != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(user.Address.Position, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));
                map.Pins.Clear();
                map.Pins.Add(new Pin { Label = address, Address = address, Position = user.Address.Position });

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
                App.Locator.MainViewModel.LoadingCount++;
                App.Locator.MainViewModel.UpdateUserLocation((res) =>
                {
                    App.Locator.MainViewModel.LoadingCount--;
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
