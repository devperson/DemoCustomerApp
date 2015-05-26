using CustomerApp.Models;
using CustomerApp.Models.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp
{
    public interface IWebServiceClient
    {
        void PostObject<T>(string requestUrl, T obj, Action<ResponseBase> onCompleted = null);
        void GetMenu(Action<MenuResponse> onCompleted);

        void RegisterUser(User user, Action<AuthResponse> action);
        void Login(object obj, Action<AuthResponse> action);

        void GetOrders(int custId, Action<OrdersResponse> onCompleted);
        void PutOrder(object obj, Action<OrderResponse> onCompleted);
    }

   
}
