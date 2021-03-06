﻿using CustomerApp.Controls.Models;
using CustomerApp.Models;
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
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            
            this.BindingContext = App.Locator.MainViewModel;

            genderPicker.Items.Add("Male");
            genderPicker.Items.Add("Female");
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var user = App.Locator.MainViewModel.User;
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            try
            {
                var geoLocation = await locator.GetPositionAsync(5000);

                var pos = new Position(geoLocation.Latitude, geoLocation.Longitude);

                var geo = new Geocoder();
                var addresses = await geo.GetAddressesForPositionAsync(pos);
                var addr = addresses.First();

                var userAddress = new Address();
                userAddress.AddressText = addr;
                userAddress.Position = pos;
                user.Address = userAddress;
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Error", "Error on getting current user location: " + ex.Message, "OK");

                return;
            }
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
                lblErr.Text = "Please fill all fields.";
                return;
            }

            App.Locator.MainViewModel.LoadingCount++;
            App.Locator.MainViewModel.WebService.RegisterUser(user, (res) =>
            {
                App.Locator.MainViewModel.LoadingCount--;
                if (res.Success)
                {
                    App.Locator.MainViewModel.User.Id = res.UserId;

                    App.Locator.MainViewModel.InitSignalRConnection();
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
