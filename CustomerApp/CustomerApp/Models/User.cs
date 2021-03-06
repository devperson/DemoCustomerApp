﻿using CustomerApp.Controls.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Models
{
    public class User
    {
        public User()
        {
            this.Address = new Address();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public Address Address { get; set; }
        //address
        //public string AddressLine1 { get; set; }
        //public string AddressLine2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Postal { get; set; }
        //public string Country { get; set; }
    }
}
