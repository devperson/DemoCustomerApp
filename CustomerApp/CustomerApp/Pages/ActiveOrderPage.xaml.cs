using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomerApp
{
    public partial class ActiveOrderPage : ContentPage
    {
        public ActiveOrderPage()
        {
            InitializeComponent();

            this.ToolbarItems.Add(new ToolbarItem("Cancel", null, () => {             
                this.DisplayAlert("Error","Action not supported yet.", "Close");
            }));

            this.Init();
        }

        private void Init()
        {
            var myPos = App.Locator.MainViewModel.User.Address.Position;
            var driverPos = new Position(App.Locator.MainViewModel.ViewOrder.Driver.Lat, App.Locator.MainViewModel.ViewOrder.Driver.Lon);

            if (myPos != null && myPos.Longitude != 0)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(myPos, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));

                map.Pins.Clear();
                bool isAddedPin = false;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (!isAddedPin)
                    {
                        isAddedPin = true;
                        map.Pins.Add(new Pin { Label = "My Location", Position = myPos });
                        map.Pins.Add(new Pin { Label = "Driver Location", Position = driverPos });
                        this.DrawRout(myPos, driverPos);
                    }

                    return false;
                });
            }   
        }

        private async void DrawRout(Position myPos, Position driverPos)
        {
            await map.CreateRoute(myPos, driverPos);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.Locator.MainViewModel.DriverPosisionChanged += MainViewModel_DriverPosisionChanged;
        }

        private void MainViewModel_DriverPosisionChanged(object sender, Position newPos)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var oldPin = map.Pins.First(p => p.Label == "Driver Location");
                map.Pins.Remove(oldPin);

                map.Pins.Add(new Pin { Label = "Driver Location", Position = newPos });

                 var myPos = App.Locator.MainViewModel.User.Address.Position;

                 this.DrawRout(myPos, newPos);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.Locator.MainViewModel.DriverPosisionChanged -= MainViewModel_DriverPosisionChanged;
        }
    }
}
