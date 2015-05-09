﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CustomerApp
{
    public partial class CheckOutPage : ContentPage
    {
        public CheckOutPage()
        {
            InitializeComponent();

            for (int i = 0; i < App.Locator.MainViewModel.Orders.Count; i++)
            {
                var orders = App.Locator.MainViewModel.Orders;
                var lblQuantity = new Label();
                lblQuantity.VerticalOptions = LayoutOptions.Center;
                lblQuantity.TextColor = Color.Red;
                lblQuantity.Text = orders[i].Quantity.ToString();
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
                lblMenu.Text = orders[i].Name;
                Grid.SetRow(lblMenu, i);
                Grid.SetColumn(lblMenu, 2);
                orderList.Children.Add(lblMenu);

                var lblPrice = new Label();
                lblPrice.VerticalOptions = LayoutOptions.Center;
                lblPrice.TextColor = Color.Red;
                lblPrice.Text = "$" + (orders[i].Quantity * orders[i].Price).ToString();
                Grid.SetRow(lblPrice, i);
                Grid.SetColumn(lblPrice, 3);
                orderList.Children.Add(lblPrice);
            }

            double tax = 4.3, subTotal = App.Locator.MainViewModel.Orders.Sum(o => (o.Quantity * o.Price));
            lblTax.Text = "$" + tax;
            lblSubTotal.Text = "$" + subTotal;
            lblTotal.Text = "$" + (subTotal + tax).ToString();
        }
    }
}
