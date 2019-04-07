using System;
using System.Collections.Generic;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class TagsPage : ContentPage
    {
        public TagsPage()
        {
            InitializeComponent();
            BindingContext = new TagsViewModel();
        }
    }
}
