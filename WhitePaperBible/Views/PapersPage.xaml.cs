using System;
using System.Collections.Generic;
using System.Linq;
using WhitePaperBible.Core.Models;
using WhitePaperBible.ViewModels;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public partial class PapersPage : ContentPage
    {
        public PapersPage()
        {
            InitializeComponent();



            BindingContext = new PapersViewModel();

            Shell.SetSearchHandler(this,
                new PaperSearchHandler() { VM = (BindingContext as PapersViewModel) });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }

    class PaperSearchHandler : SearchHandler
    {
        public PapersViewModel VM { get; set; }

        public PaperSearchHandler()
        {
            SearchBoxVisibility = SearchBoxVisibility.Expanded;
            ShowsResults = false;
        }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            if (string.IsNullOrEmpty(newValue))
            {
                //ItemsSource = null;
                VM.FilterPapers(string.Empty);
            }
            else
            {
                VM.FilterPapers(newValue);
                //var results = new List<Paper>();
                //results = VM
                //    .Papers
                //    .Where(x => x.title.IndexOf(newValue, StringComparison.InvariantCultureIgnoreCase) > -1)
                //    .ToList<Paper>();
                //ItemsSource = results;
            }
        }

        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
        }
    }
}
