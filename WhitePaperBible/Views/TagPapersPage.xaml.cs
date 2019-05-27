using System;
using System.Collections.Generic;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class TagPapersPage : ContentPage
    {
        public TagPapersPage()
        {
            InitializeComponent();

            BindingContext = new TagPapersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.NavigationRoot = this;
        }
    }
}
