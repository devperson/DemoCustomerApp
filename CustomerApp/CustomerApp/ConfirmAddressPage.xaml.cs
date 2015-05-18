﻿using CustomerApp.Controls.Models;
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


            var pos = App.Locator.MainViewModel.User.UserAddress.Position;
            var address = App.Locator.MainViewModel.User.UserAddress.AddressText;
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

        private async void DisplayAlert(string address)
        {
            var result = await this.DisplayAlert("Delivery Address", address, "Confirm", "Cancel");
            if (result)
            {                
                App.Current.MainPage = new MainPage();
            }
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
                App.Current.MainPage = new MainPage();
            }
        }
    }
}
