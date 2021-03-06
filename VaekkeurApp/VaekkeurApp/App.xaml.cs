﻿using MediaManager;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VaekkeurApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CrossMediaManager.Current.Init();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
