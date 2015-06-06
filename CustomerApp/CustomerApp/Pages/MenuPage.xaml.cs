using CustomerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class MenuPage : ContentPage
    {        
        public MenuPage()
        {
            InitializeComponent();

            this.BindingContext = App.Locator.MainViewModel;

            listView.ItemSelected += (s, e) =>
            {
                listView.SelectedItem = null;
            };

            TapGestureRecognizer g = new TapGestureRecognizer();
            g.Tapped += checkOut_Tapped;
            checkOutContent.GestureRecognizers.Add(g);

            App.Locator.MainViewModel.ShowAlert = this.DisplayAlert;
        }

        private void checkOut_Tapped(object sender, EventArgs e)
        {
            var content = sender as ContentView;
            if(App.Locator.MainViewModel.IsCheckOutEnabled)
            {
                content.Opacity = 0.7;                
                this.Navigation.PushAsync(new CheckOutPage());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            checkOutContent.Opacity = 1;            
        }
    }
}
