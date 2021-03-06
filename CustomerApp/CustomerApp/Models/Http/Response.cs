﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace CustomerApp.Models.Http
{
    public class ResponseBase
    {        
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ErrorResponseModel
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }

        public bool HasErrorMessage
        {
            get
            {
                return !string.IsNullOrEmpty(this.Message) || !string.IsNullOrEmpty(this.ExceptionMessage);
            }
        }

        public string ErrorMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Message))
                    return this.Message;
                else
                    return this.ExceptionMessage;
            }
        }
    }

    public class MenuResponse : ResponseBase
    {
        public List<Menu> Menu { get; set; }
    }

    public class AuthResponse : ResponseBase
    {
        public int UserId { get; set; }
    }

    public class OrderResponse : ResponseBase
    {
        public int OrderId { get; set; }
        public int DriverId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }        
    }

    public class OrdersResponse : ResponseBase
    {
        public List<Order> Orders { get; set; }
    }

    public class LocationResponse : ResponseBase
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Address { get; set; }
    }
}
