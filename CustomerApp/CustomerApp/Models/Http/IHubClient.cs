using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace CustomerApp
{
    public interface IHubClient
    {
        void NotifyNewOrderPosted(MsgData args);
        void Initialize(string host, string clientName);
        event EventHandler<DriverEventArgs> OnDriverPositionChanged;
        event EventHandler<OrderEventArgs> OnOrderCompleted;
    }

    public class OrderEventArgs : EventArgs
    {
        public int OrderId { get; set; }
    }

    public class DriverEventArgs : EventArgs
    {
        public int DriverID { get; set; }
        public Position Position { get; set; }
    }

    public class MsgData
    {
        public MsgData()
        {
            this.To = new List<string>();
        }
        public List<string> To { get; set; }
        public object Data { get; set; }
    }
}
