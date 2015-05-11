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
            if (pos != null && pos.Longitude != 0)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Xamarin.Forms.Maps.Distance.FromMiles(0.5)));

                map.Pins.Clear();
                map.Pins.Add(new Pin { Label = "My Location", Position = pos });
                map.Pins.Add(new Pin { Label = "Driver Location", Position = new Position(pos.Latitude + 3, pos.Longitude + 2) });
            }

            this.UpdateDriverLocation();

            //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            //{
            //    this.Navigation.RemovePage(CheckOutPage.Instance);
            //    CheckOutPage.Instance = null;
            //    return false;
            //});


        }

        bool keepTimer = true;
        private void UpdateDriverLocation()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                return keepTimer;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            keepTimer = false;
        }
    }
}
