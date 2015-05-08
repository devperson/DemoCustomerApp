using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Controls.Models
{
    public class Address
    {
        public string AddressText { get; set; }
        public Xamarin.Forms.Maps.Position Position { get; set; }
    }
}
