using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CustomerApp
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new LoginPage());
        }

        private void btnReg_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new RegistrationPage());
        }
    }
}
