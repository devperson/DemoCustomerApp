using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp
{
    public class MainPage : MasterDetailPage
    {        
        public MainPage()
        {
            this.Title = "Meals";
            var sideList = new SideBarList();
            sideList.SideItemSelected += (s, e) =>
            {
                if (e == "Meals" && !(this.Detail is MenuPage))
                {
                    this.Detail = new NavigationPage(new MenuPage());
                    this.Title = "Meals";
                }
                if (e == "Orders" && !(this.Detail is OrdersPage))
                {
                    this.Detail = new NavigationPage(new OrdersPage());
                    this.Title = "Orders";
                }
                if (e == "My Account" && !(this.Detail is MyAccountPage))
                {
                    this.Detail = new NavigationPage(new MyAccountPage());
                    this.Title = "My Account";
                }
                if (e == "Logout")
                    App.Current.MainPage = new NavigationPage(new StartPage());

                this.IsPresented = false;
            };
            sideList.Icon = "settings.png";

            this.Master = sideList;
            this.Detail = new NavigationPage(new MenuPage());

            App.Locator.MainViewModel.ShowAlert = this.DisplayAlert;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
