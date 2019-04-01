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

            App.NavigationRoot = this;

            BindingContext = new PapersViewModel();

            Shell.SetSearchHandler(this,
                new PaperSearchHandler() { VM = (BindingContext as PapersViewModel) });
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var handler = Shell.GetSearchHandler(this);
            handler.SearchBoxVisibility = (handler.SearchBoxVisibility == SearchBoxVisiblity.Hidden) ? SearchBoxVisiblity.Expanded : SearchBoxVisiblity.Hidden;
        }
    }

    class PaperSearchHandler : SearchHandler
    {
        public PapersViewModel VM { get; set; }

        public PaperSearchHandler()
        {
            SearchBoxVisibility = SearchBoxVisiblity.Expanded;
            ShowsResults = false;
            //Placeholder = "Find a seashell...";
            //this.QueryIcon = ImageSource.FromResource("search.png");

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
    }
}
