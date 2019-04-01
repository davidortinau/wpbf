using System;
using System.Collections.Generic;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class PaperDetailPage : ContentPage
    {
        public string ID { get; set; }

        public PaperDetailPage()
        {
            InitializeComponent();

            App.NavigationRoot = this;

            BindingContext = new PaperDetailViewModel();

            //var bb = new BackButtonBehavior()
            //{
            //    TextOverride = "Back",
            //    Command = new Command((obj) =>
            //    {
            //        this.Navigation.PopAsync();
            //    })
            //};

            //Shell.SetBackButtonBehavior(this, bb);

        }
    }
}
