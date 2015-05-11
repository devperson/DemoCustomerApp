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
            var sideList = new SideBarList();
            this.Master = sideList;
            this.Detail = new MenuPage();

            sideList.SideItemSelected += (s, e) =>
            {
                if (e == "Meals" && !(this.Detail is MenuPage))
                    this.Detail = new MenuPage();
                if (e == "Orders" && !(this.Detail is OrdersPage))
                    this.Detail = new OrdersPage();

                this.IsPresented = false;
            };
        }
    }
}
