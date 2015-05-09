using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CustomerApp
{
    public partial class SideBarList : ContentPage
    {
        public SideBarList()
        {
            InitializeComponent();

            listView.ItemsSource = new List<string> { "Open Orders", "My Account", "Logout" };
        }
    }
}
