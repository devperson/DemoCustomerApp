using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomerApp
{
    public partial class CheckOutPage : ContentPage
    {

        public CheckOutPage()
        {
            InitializeComponent();

            this.BindingContext = App.Locator.MainViewModel;
            var meals = App.Locator.MainViewModel.CurrentOrder.Meals;
            for (int i = 0; i < meals.Count; i++)
            {
                var orders = App.Locator.MainViewModel.Orders;
                var lblQuantity = new Label();
                lblQuantity.VerticalOptions = LayoutOptions.Center;
                lblQuantity.TextColor = Color.Red;
                lblQuantity.Text = meals[i].Quantity.ToString();
                lblQuantity.FontSize = 19;
                Grid.SetRow(lblQuantity, i);                
                orderList.Children.Add(lblQuantity);

                var lbl = new Label();
                lbl.VerticalOptions = LayoutOptions.Center;
                lbl.TextColor = Color.Black;
                lbl.Text = " x ";
                Grid.SetColumn(lbl, 1);
                Grid.SetRow(lbl, i);
                orderList.Children.Add(lbl);

                var lblMenu = new Label();
                lblMenu.VerticalOptions = LayoutOptions.Center;
                lblMenu.TextColor = Color.Black;
                lblMenu.FontSize = 16;
                lblMenu.Text = meals[i].Name;
                Grid.SetRow(lblMenu, i);
                Grid.SetColumn(lblMenu, 2);
                orderList.Children.Add(lblMenu);

                var lblPrice = new Label();
                lblPrice.VerticalOptions = LayoutOptions.Center;
                lblPrice.TextColor = Color.Red;
                lblPrice.Text = "$" + (meals[i].Quantity * meals[i].Price).ToString();
                Grid.SetRow(lblPrice, i);
                Grid.SetColumn(lblPrice, 3);
                orderList.Children.Add(lblPrice);
            }

            double tax = 4.3, subTotal = App.Locator.MainViewModel.CurrentOrder.Meals.Sum(o => (o.Quantity * o.Price));
            lblTax.Text = "$" + tax;
            lblSubTotal.Text = "$" + subTotal;
            lblTotal.Text = "$" + (subTotal + tax).ToString();
        }

        public void confirm_Clicked(object sender, EventArgs e)
        {
            App.Locator.MainViewModel.SendOrder((res) =>
            {
                if (res.Success)
                {
                    this.Navigation.RemovePage(this);
                    this.Navigation.PushAsync(new ActiveOrderPage());
                }
                else
                {
                    this.DisplayAlert("Error", "Error on sending order. " + res.Error, "Close");
                    this.Navigation.PopAsync(true);
                }
            });
        }
    }
}
