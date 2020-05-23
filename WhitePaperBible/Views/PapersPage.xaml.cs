using System;
using System.Collections.Generic;
using System.Linq;
using WhitePaperBible.Core.Models;
using WhitePaperBible.ViewModels;
using WhitePaperBible.Views.Templates;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class PapersPage : ContentPage
    {
        public PapersPage()
        {
            InitializeComponent();

            BindingContext = new PapersViewModel();

            //Shell.SetSearchHandler(this,
            //    new PaperSearchHandler() { VM = (BindingContext as PapersViewModel) });
        }

        void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                //ItemsSource = null;
                (BindingContext as PapersViewModel).FilterPapers(string.Empty);
            }
            else
            {
                (BindingContext as PapersViewModel).FilterPapers(e.NewTextValue);
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            SearchBar.IsVisible = !SearchBar.IsVisible;
        }
    }

    class PaperSearchHandler : SearchHandler
    {
        public PapersViewModel VM { get; set; }

        public PaperSearchHandler()
        {
            SearchBoxVisibility = SearchBoxVisibility.Expanded;
            ShowsResults = true;
            this.ItemTemplate = new DataTemplate(typeof(PaperItemTemplate));
            
        }


        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if(ItemsSource == null)
            {
                ItemsSource = VM.Papers;
            }

            if (string.IsNullOrEmpty(newValue))
            {
                //ItemsSource = null;
                VM.FilterPapers(string.Empty);
            }
            else
            {
                VM.FilterPapers(newValue);
            }
        }

        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            VM.PaperSelectedCommand.Execute((Paper)item);
        }
    }
}
