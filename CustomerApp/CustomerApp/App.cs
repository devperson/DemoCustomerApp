﻿using CustomerApp.Models;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CustomerApp
{
    public class App : Application
    {
        public static ViewModelLocator Locator { get; set; }        
        public App()
        {
            Locator = new ViewModelLocator();
            // The root page of your application
            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
