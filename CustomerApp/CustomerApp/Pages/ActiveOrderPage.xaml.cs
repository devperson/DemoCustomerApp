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

            this.ToolbarItems.Add(new ToolbarItem("Cancel", null, () => { }));

            this.Init();
        }

        private void Init()
        {
            var pos = App.Locator.MainViewModel.User.UserAddress.Position;
            var driver = App.Locator.MainViewModel.ViewOrder.Driver;

            if (pos != null && pos.Longitude != 0)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));

                map.Pins.Clear();
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    map.Pins.Add(new Pin { Label = "My Location", Position = pos });
                    map.Pins.Add(new Pin { Label = "Driver Location", Position = new Position(driver.Lat, driver.Lon) });

                    return false;
                });
            }   
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
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.Locator.MainViewModel.DriverPosisionChanged -= MainViewModel_DriverPosisionChanged;
        }
    }
}
