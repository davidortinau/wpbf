﻿using System;
using System.Collections.Generic;
using WhitePaperBible.Views;
using Xamarin.Forms;

namespace WhitePaperBible
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute("paper", typeof(PaperDetailPage));
        }

        void Handle_Navigating(object sender, Xamarin.Forms.ShellNavigatingEventArgs e)
        {
            Console.WriteLine($"SOURCE: {e?.Source.ToString()} | CURRENT: {e.Current?.Location.ToString()} | TARGET: {e.Target?.Location.ToString()}");

        }


    }
}