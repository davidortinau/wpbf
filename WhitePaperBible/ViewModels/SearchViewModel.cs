using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WhitePaperBible.Core;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using WhitePaperBible.Enums;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Command DoSearchCommand { get; set; }

        public Command ChangeScopeCommand { get; set; }

        public List<ReferenceNode> Results
        {
            get; set;
        }

        public string SearchTerms { get; set; }

        private IJSONWebClient _client;

        public SearchViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();
            DoSearchCommand = new Command(PerformSearch);
            ChangeScopeCommand = new Command<string>(ChangeScope);
        }

        private void ChangeScope(string s)
        {
            switch (s)
            {
                case "reference": scope = SearchScopeEnum.Reference;break;
                case "keyword": scope = SearchScopeEnum.Keyword;break;
                default:
                    scope = SearchScopeEnum.Phrase; break;
            }
        }

        SearchScopeEnum scope = SearchScopeEnum.Reference;

        async void PerformSearch()
        {
            //var args = new GetBibleSearchResultsInvokerArgs(SearchBar.Text, (int)SearchBar.SelectedScopeButtonIndex);
            //DoSearch.Invoke(args);

            //SearchBar.ResignFirstResponder();
            if (scope is SearchScopeEnum.Reference)
            {
                await _client.OpenURL(Constants.BASE_URI + String.Format("search/by_reference.json?keywords={0}&caller=wpb-iPhone", SearchTerms));
            }
            else if (scope is SearchScopeEnum.Keyword)
            {
                await _client.OpenURL(Constants.BASE_URI + String.Format("search/by_keyword.json?keywords={0}&caller=wpb-iPhone", SearchTerms));
            }
            else
            {
                await _client.OpenURL(Constants.BASE_URI + String.Format("search/by_phrase.json?keywords={0}&caller=wpb-iPhone", SearchTerms));
            }

            var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReferenceNode>>(_client.ResponseText);

            Results = payload;
        }

        //public void Execute(InvokerArgs args)
        //{
        //    Service.Success += onSuccess;
        //    var bibleArgs = (args as GetBibleSearchResultsInvokerArgs);
        //    Service.Execute(bibleArgs.Keywords, bibleArgs.Scope);
        //}

        //void onSuccess(object sender, EventArgs args)
        //{
        //    var a = ((BibleSearchServiceEventArgs)args).Results;
        //    Received.Invoke(new BibleSearchResultsReceivedInvokerArgs(a));
        //}
    }
}
