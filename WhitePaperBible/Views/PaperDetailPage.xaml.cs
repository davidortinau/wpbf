using System;
using System.Collections.Generic;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class PaperDetailPage : ContentPage
    {
        public string ID { get; set; }

        public PaperDetailViewModel VM { get; set; }

        public PaperDetailPage()
        {
            InitializeComponent();

            BindingContext = VM = new PaperDetailViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationRoot = this;

            VM.ID = ID;
        }
    }
}
