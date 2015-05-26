//using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CustomerApp.Models;
using CustomerApp.Models.Http;


[assembly: Xamarin.Forms.Dependency(typeof(CustomerApp.PCL.WebService))]
namespace CustomerApp.PCL
{
    /// <summary>
    /// This class provides functions for accessing data service.
    /// </summary>
    public class WebService : IWebServiceClient
    {
        private RestClient client;        
        protected RestClient Client
        {
            get
            {
                return client;
            }
        }
        public WebService()
		{

			var url = App.Locator.MainViewModel.ApiUrl;
            client = new RestClient(url); 
			client.AddDefaultHeader("Accept", "application/json");
		}

        public async void RegisterUser(User user, Action<AuthResponse> action)
        {
            var asyncResult = await ExecuteServiceMethod<AuthResponse>("api/customerapi/register", Method.POST, content =>
            {                
                var response = JsonConvert.DeserializeObject<AuthResponse>(content);
                return response;
            }, user);
            if (action != null)
                action(asyncResult);
        }

        public async void Login(object obj, Action<AuthResponse> action)
        {
            var asyncResult = await ExecuteServiceMethod<AuthResponse>("api/customerapi/login", Method.POST, content =>
            {
                var response = JsonConvert.DeserializeObject<AuthResponse>(content);
                return response;
            }, obj);
            if (action != null)
                action(asyncResult);
        }

       

        public async void GetMenu(Action<MenuResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<MenuResponse>("api/customerapi/getmenu", Method.GET, content =>
            {
                var response = new MenuResponse { Result = JsonConvert.DeserializeObject<List<Menu>>(content), Success = true };
                return response;
            });
            if (onCompleted != null)
                onCompleted(asyncResult);
        }

        public async void PutOrder(object obj, Action<OrderResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<OrderResponse>("api/customerapi/Order", Method.POST, content =>
            {
                var response = JsonConvert.DeserializeObject<OrderResponse>(content);
                return response;
            }, obj);
            if (onCompleted != null)
                onCompleted(asyncResult);
        }

        public async void GetOrders(int cusId, Action<OrdersResponse> onCompleted)
        {
            var asyncResult = await ExecuteServiceMethod<OrdersResponse>("api/customerapi/GetOrders?customerId=" + cusId, Method.GET, content =>
            {
                var response = new OrdersResponse { Orders = JsonConvert.DeserializeObject<List<Order>>(content), Success = true };
                return response;
            });
            if (onCompleted != null)
                onCompleted(asyncResult);
        }

        /// <summary>
        /// Method provides register object service call.
        /// </summary>    
        public async void PostObject<T>(string requestUrl, T obj, Action<ResponseBase> onCompleted = null)
        {
            var asyncResult = await ExecuteServiceMethod<ResponseBase>(requestUrl, Method.POST, content =>
            {
                var response = new ResponseBase();
                return response;
            }, obj);
            if (onCompleted != null)
                onCompleted(asyncResult);
        }

       
        /// <summary>
        /// Helper method for sending http commands.
        /// </summary>        
        public Task<T> ExecuteServiceMethod<T>(string resource, Method method, Func<string, T> deserialiser, object requestObject = null) where T : ResponseBase
        {
            var restRequest = new RestRequest(resource, method);
            if (requestObject != null)
            {
                restRequest.RequestFormat = DataFormat.Json;
                var json = JsonConvert.SerializeObject(requestObject);
                restRequest.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                //restRequest.AddBody(requestObject);
            }

            return Task.Run<T>(() =>
            {
                T response = Activator.CreateInstance<T>();
                var errorResponse = new ErrorResponseModel();
                try
					{
                    var restResponse = Client.Execute(restRequest);
                    this.CheckServer(restResponse.Content);
                    if (!string.IsNullOrEmpty(restResponse.Content))
                    {
                        response = deserialiser(restResponse.Content);// JsonConvert.DeserializeObject<T>(restResponse.Content);
                        if (restResponse.Content.Contains("ExceptionMessage"))
                            errorResponse = JsonConvert.DeserializeObject<ErrorResponseModel>(restResponse.Content);
                        else response.Success = true;
                    }
                    else
                    {                        
                        errorResponse.ExceptionMessage = "No connection";
					    response.Success = false;
                    }
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.ExceptionMessage))
                    {
                        response.Success = false;
                        response.Error = errorResponse.ExceptionMessage;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Error = "Server is down please try later.";
                }
                return response;
            });
        }

        /// <summary>
        /// Helper method for validating service result.
        /// </summary>        
        public void CheckServer(string responsString)
        {
            string htmlContent = "<!DOCTYPE";
            if (responsString.Contains(htmlContent))
                throw new Exception("Server is down please try later.");
        }        
    }
}