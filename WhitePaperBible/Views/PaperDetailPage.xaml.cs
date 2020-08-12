using System;
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
            if (isScrollingUp && ContentWebView.ScrollY > 60)
            {
                // hide the nav bar and toolbar
                Shell.SetNavBarIsVisible(this, false);
                //BottomToolbar.IsVisible = false;
                lastScrollY = ContentWebView.ScrollY;
            }
            else
            {
                Debug.WriteLine($"Dif > 100: {(lastScrollY - ContentWebView.ScrollY)}");
                if (ContentWebView.ScrollY <= 0 || (lastScrollY - ContentWebView.ScrollY) > 200)
                {
                    Shell.SetNavBarIsVisible(this, true);
                    //BottomToolbar.IsVisible = true;
                    lastScrollY = ContentWebView.ScrollY;
                }
            }


        }


        uint duration = 100;

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            

            if (Backdrop.Opacity == 0)
            {
                await Backdrop.FadeTo(1, length:duration);
                await BottomToolbar.TranslateTo(0, 0, length: duration, easing:Easing.SinIn);
            }
            else
            {
                await Backdrop.FadeTo(0, length: duration);
                await BottomToolbar.TranslateTo(0, 220, length: duration,easing:Easing.SinIn);
            }
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            if (isBackdropTapEnabled)
            {
                await Backdrop.FadeTo(0, length: duration);
                await BottomToolbar.TranslateTo(0, 220, length: duration, easing: Easing.SinIn);
            }
        }

        double lastPanY = 0;
        bool isBackdropTapEnabled = true;
        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            if(e.StatusType == GestureStatus.Running)
            {
                isBackdropTapEnabled = false;
                lastPanY = e.TotalY;
                Debug.WriteLine($"R: {e.TotalY}");
                if (e.TotalY > 0)
                {
                    BottomToolbar.TranslationY = e.TotalY;
                }
                
            }
            else if(e.StatusType == GestureStatus.Completed)
            {
                Debug.WriteLine($"Completed: {e.TotalY}");
                if(lastPanY < 110)
                {
                    await BottomToolbar.TranslateTo(0, 0, length: duration, easing: Easing.SinIn);
                }
                else
                {
                    await Backdrop.FadeTo(0, length: duration);
                    await BottomToolbar.TranslateTo(0, 220, length: duration, easing: Easing.SinIn);
                }
                isBackdropTapEnabled = true;
                //lastPanY = 0;
            }
        }
    }
}
