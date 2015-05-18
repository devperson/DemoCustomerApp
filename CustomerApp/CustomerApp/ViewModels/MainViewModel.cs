using CustomerApp.Models;
using Geolocator.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomerApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {        
        private bool _isCheckOutEnabled;
        public bool IsCheckOutEnabled
        {
            get { return _isCheckOutEnabled; }
            set
            {
                if (value != _isCheckOutEnabled)
                {
                    _isCheckOutEnabled = value;
                    this.RaisePropertyChanged(p => p.IsCheckOutEnabled);
                }
            }
        }

        public User User { get; set; }
        //public ObservableCollection<Menu> Orders { get; set; }
        public Order ViewOrder { get; set; }

        Order _currentOrder;
        public Order CurrentOrder 
        {
            get { return _currentOrder; }
            set
            {
                if (_currentOrder != null)
                    _currentOrder.Meals.CollectionChanged -= Meals_CollectionChanged;
                _currentOrder = value;
                _currentOrder.Meals.CollectionChanged += Meals_CollectionChanged;                
            }
        }
        public ObservableCollection<Order> Orders { get; set; }        
        public ObservableCollection<Menu> Menu { get; set; }
        
        public MainViewModel()
        {
            this.User = new User();
            this.Menu = new ObservableCollection<Menu>();
            this.Orders = new ObservableCollection<Order>();
            this.CurrentOrder = new Order();
            this.CurrentOrder.Meals.CollectionChanged += Meals_CollectionChanged;

            Menu menu1 = new Menu();
            menu1.Name = "CHICKEN AND CHEESE ENCHILADAS";
            menu1.Price = 8;
            menu1.Description = "homemade chicken & cheese enchiladas with salsa roja, spanish rice, pinto beans & corn (~750 cal)";
            menu1.Image = "img1.jpg";//"http://localhost:1732/Images/Products/img1.jpg";
            menu1.AvailableDate = DateTime.Now;
            this.Menu.Add(menu1);

            var menu2 = new Menu();
            menu2.Name = "THAI STYLE PORK RICE BOWL";
            menu2.Price = 10;
            menu2.Description = "spicy minced pork with chilies, mint, lime, bell peppers, steamed jasmine rice & sauteed green beans (~425 cal)";
            menu2.Image = "img2.jpg";//"http://localhost:1732/Images/Products/img2.jpg";
            menu2.AvailableDate = DateTime.Now;
            this.Menu.Add(menu2);

            var menu3 = new Menu();
            menu3.Name = "FOUR CHEESE RAVIOLI WITH WILD MUSHROOM SAUCE";
            menu3.Price = 7;
            menu3.Description = "four cheese ravioli with wild mushroom sauce, asparagus, peas, zucchini, sun dried tomatoes & fontina cheese (~700 cal)";
            menu3.Image = "img3.jpg";//"http://localhost:1732/Images/Products/img3.jpg";
            menu3.AvailableDate = DateTime.Now;
            this.Menu.Add(menu3);
        }

        private void Meals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.IsCheckOutEnabled = this.CurrentOrder.Meals.Count > 0;
        }

        public void ClearMenuSelections()
        {            
            foreach (var item in this.Menu)
            {
                item.Quantity = 0;
            }

            this.IsCheckOutEnabled = false;
        }
    }
}
