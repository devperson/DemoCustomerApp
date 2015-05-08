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
        List<Menu> menus = new List<Menu>();
        public MenuPage()
        {
            InitializeComponent();


            Menu menu1 = new Menu();
            menu1.Name = "CHICKEN AND CHEESE ENCHILADAS";
            menu1.Price = 8;
            menu1.Description = "homemade chicken & cheese enchiladas with salsa roja, spanish rice, pinto beans & corn (~750 cal)";
            menu1.Image = "img1.jpg";//"http://localhost:1732/Images/Products/img1.jpg";
            menu1.AvailableDate = DateTime.Now;
            menus.Add(menu1);

            var menu2 = new Menu();
            menu2.Name = "THAI STYLE PORK RICE BOWL";
            menu2.Price = 10;
            menu2.Description = "spicy minced pork with chilies, mint, lime, bell peppers, steamed jasmine rice & sauteed green beans (~425 cal)";
            menu2.Image = "img2.jpg";//"http://localhost:1732/Images/Products/img2.jpg";
            menu2.AvailableDate = DateTime.Now;
            menus.Add(menu2);

            var menu3 = new Menu();
            menu3.Name = "FOUR CHEESE RAVIOLI WITH WILD MUSHROOM SAUCE";
            menu3.Price = 7;
            menu3.Description = "four cheese ravioli with wild mushroom sauce, asparagus, peas, zucchini, sun dried tomatoes & fontina cheese (~700 cal)";
            menu3.Image = "img3.jpg";//"http://localhost:1732/Images/Products/img3.jpg";
            menu3.AvailableDate = DateTime.Now;
            menus.Add(menu3);

            listView.ItemsSource = menus;

            listView.ItemSelected += (s, e) =>
            {
                listView.SelectedItem = null;
            };
        }
    }
}
