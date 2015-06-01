using CustomerApp.Models;
using CustomerApp.Models.Http;
using Geolocator.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CustomerApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public string ApiUrl = "";
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

        private IWebServiceClient _service;
        public IWebServiceClient WebService
        {
            get
            {
                if (_service == null)
                {
                    _service = DependencyService.Get<IWebServiceClient>();                    
                }
                return _service;
            }
        }

        private IHubClient _ntf;
        public IHubClient Notifier
        {
            get
            {
                if (_ntf == null)
                {
                    _ntf = DependencyService.Get<IHubClient>();
                }
                return _ntf;
            }
        }
        
        public MainViewModel()
        {
            if (Debugger.IsAttached)
                this.ApiUrl = "http://xusanpc:1732/";
            else
                this.ApiUrl = "http://demowebserver.apphb.com/";
            this.User = new User();
            this.Menu = new ObservableCollection<Menu>();
            this.Orders = new ObservableCollection<Order>();
            this.CurrentOrder = new Order();
            this.CurrentOrder.Meals.CollectionChanged += Meals_CollectionChanged;

            //Menu menu1 = new Menu();
            //menu1.Name = "CHICKEN AND CHEESE ENCHILADAS";
            //menu1.Price = 8;
            //menu1.Description = "homemade chicken & cheese enchiladas with salsa roja, spanish rice, pinto beans & corn (~750 cal)";
            //menu1.Image = "img1.jpg";//"http://localhost:1732/Images/Products/img1.jpg";
            ////menu1.AvailableDate = DateTime.Now;
            //this.Menu.Add(menu1);

            //var menu2 = new Menu();
            //menu2.Name = "THAI STYLE PORK RICE BOWL";
            //menu2.Price = 10;
            //menu2.Description = "spicy minced pork with chilies, mint, lime, bell peppers, steamed jasmine rice & sauteed green beans (~425 cal)";
            //menu2.Image = "img2.jpg";//"http://localhost:1732/Images/Products/img2.jpg";
            ////menu2.AvailableDate = DateTime.Now;
            //this.Menu.Add(menu2);

            //var menu3 = new Menu();
            //menu3.Name = "FOUR CHEESE RAVIOLI WITH WILD MUSHROOM SAUCE";
            //menu3.Price = 7;
            //menu3.Description = "four cheese ravioli with wild mushroom sauce, asparagus, peas, zucchini, sun dried tomatoes & fontina cheese (~700 cal)";
            //menu3.Image = "img3.jpg";//"http://localhost:1732/Images/Products/img3.jpg";
            ////menu3.AvailableDate = DateTime.Now;
            //this.Menu.Add(menu3);
        }

        public void OnUserLogedIn()
        {
            this.Notifier.Initialize(this.ApiUrl, "Customer" + this.User.Id.ToString());
            this.Notifier.OnDriverPositionChanged += Notifier_OnDriverPositionChanged;
            this.Notifier.OnOrderCompleted += Notifier_OnOrderCompleted;
            this.GetData();
        }

        private void GetData()
        {
            this.WebService.GetMenu((response) =>
            {
                this.Menu = new ObservableCollection<Menu>(response.Menu);
                this.RaisePropertyChanged(p => p.Menu);
                this.WebService.GetOrders(this.User.Id, (res) =>
                {
                    foreach (var or in res.Orders)
                    {
                        foreach (var meal in or.Meals)
                        {
                            meal.Name = this.Menu.First(m => m.Id == meal.Id).Name;
                            meal.Description = this.Menu.First(m => m.Id == meal.Id).Name;
                            meal.Price = this.Menu.First(m => m.Id == meal.Id).Price;
                            meal.Image = this.Menu.First(m => m.Id == meal.Id).Image;
                        }
                    }

                    this.Orders = new ObservableCollection<Order>(res.Orders);
                });
            });
        }

        private void Notifier_OnDriverPositionChanged(object sender, DriverEventArgs e)
        {
            var orders = this.Orders.Where(o => o.Driver.Id == e.DriverID).ToList();
            foreach (var or in orders)
            {
                or.Driver.Position = e.Position;
            }
        }

        private void Notifier_OnOrderCompleted(object sender, OrderEventArgs e)
        {
            var order = this.Orders.FirstOrDefault(or => or.Id == e.OrderId);
            if (order != null)
                order.IsDelivered = true;
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


        public void UpdateUserLocation(Action<ResponseBase> action)
        {
            this.WebService.PostObject("api/customerapi/UpdateUserLocation", new { UserId = this.User.Id, Position = this.User.UserAddress.Position, Address = this.User.UserAddress.AddressText }, action);
        }

        public void SendOrder(Action<OrderResponse> action)
        {
            var order = new { CustomerId = this.User.Id, Details = this.ViewOrder.Meals.Select(m => new { Id = m.Id, Qty = m.Quantity }).ToList() };

            this.WebService.PutOrder(order, action);
        }        
    }
}
