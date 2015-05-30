using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerApp.Models.Http;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms.Maps;

[assembly: Xamarin.Forms.Dependency(typeof(CustomerApp.Hub.HubClient))]
namespace CustomerApp.Hub
{
    public class HubClient : IHubClient
    {
        HubConnection connection;
        IHubProxy serverHub;
        public event EventHandler<DriverEventArgs> OnDriverPositionChanged;
        public event EventHandler<OrderEventArgs> OnOrderCompleted;

        public async void Initialize(string host, string clientName)
        {
            connection = new HubConnection(host, string.Format("name={0}", clientName));
            serverHub = connection.CreateHubProxy("HubServer");
            serverHub.On<DriverEventArgs>("NewDriverLocation", (e) =>
            {
                if (this.OnDriverPositionChanged != null)
                    this.OnDriverPositionChanged(this, e);
            });
            serverHub.On<OrderEventArgs>("OrderCompleted", (e) =>
            {
                if (this.OnOrderCompleted != null)
                    this.OnOrderCompleted(this, e);
            });
            await connection.Start();
        }

        public async void NotifyNewOrderPosted(OrderEventArgs args)
        {
            await serverHub.Invoke("Notify_NewOrderPosted", new object[] { args });
        }
    }
}