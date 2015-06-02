using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace CustomerApp.Models
{
    public class Order
    {
        public Order()
        {            
            this.Meals = new ObservableCollection<Menu>();
            this.Driver = new Driver();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<Menu> Meals { get; set; }

        public bool IsDelivered { get; set; }

        public Driver Driver { get; set; }
    }

    public enum OrderStatus
    {
        Open,
        Closed
    }

    public class Driver
    {
        public int Id { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }        
    }
}
