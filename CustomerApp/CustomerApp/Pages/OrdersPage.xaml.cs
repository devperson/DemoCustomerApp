using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class OrdersPage : ContentPage
    {
        public OrdersPage()
        {
            InitializeComponent();

            this.BindingContext = App.Locator.MainViewModel;
            listView.ItemTapped += (s, e) =>
            {
                var order = listView.SelectedItem as Order;
                App.Locator.MainViewModel.ViewOrder = order;
                if (!order.IsDelivered)
                    this.Navigation.PushAsync(new ActiveOrderPage());
                else
                    this.Navigation.PushAsync(new OrderPage());
            };            
        }
    }
}
