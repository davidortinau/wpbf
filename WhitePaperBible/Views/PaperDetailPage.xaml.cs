﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class PaperDetailPage : ContentPage
    {
        public PaperDetailPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsContentLoaded = true;
        }


        double lastScrollY = 0;
        private bool IsContentLoaded;

        void Handle_Scrolled(object sender, System.EventArgs e)
        {
            if (!IsContentLoaded)
                return;

            var isScrollingUp = lastScrollY < ContentWebView.ScrollY;
            if (isScrollingUp)
            {
                // hide the nav bar and toolbar
                Shell.SetNavBarIsVisible(this, false);
                BottomToolbar.IsVisible = false;
                lastScrollY = ContentWebView.ScrollY;
            }
            else
            {
                Debug.WriteLine($"Dif > 100: {(lastScrollY - ContentWebView.ScrollY)}");
                if ((lastScrollY - ContentWebView.ScrollY) > 100)
                {
                    Shell.SetNavBarIsVisible(this, true);
                    BottomToolbar.IsVisible = true;
                    lastScrollY = ContentWebView.ScrollY;
                }
            }


        }
    }
}
