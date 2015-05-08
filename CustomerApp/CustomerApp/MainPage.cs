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
            this.Master = new SideBarList();
            this.Detail = new MenuPage();
        }
    }
}
