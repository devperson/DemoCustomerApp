using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Models
{
    public class Order
    {
        public Order()
        {            
            this.Meals = new ObservableCollection<Menu>();         
        }

        public DateTime Date { get; set; }
        public ObservableCollection<Menu> Meals { get; set; }

        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Open,
        Closed
    }
}
