using MonkeyCache.SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using TinyMessenger;
using WhitePaperBible.Core;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using WhitePaperBible.ViewModels.Messages;
using WhitePaperBible.Views;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    [QueryProperty(nameof(Refresh), "refresh")]
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        TinyMessengerHub _hub;

        public ObservableCollection<Paper> Papers
        {
            get
            {
                if (string.IsNullOrEmpty(_keywords))
                {
                    return _papers;
                }
                else
                {
                    return new ObservableCollection<Paper>(_papers
                    .Where(x => x.title.IndexOf(_keywords, StringComparison.InvariantCultureIgnoreCase) > -1)
                    .ToList<Paper>());
                }
            }
            set
            {

                _papers = value;
            }
        }

        /// <summary>
        /// Anything set here will prompt a refresh.
        /// </summary>
        public string Refresh
        {
            set
            {
                FetchPapers();
            }
        }

        public Command<Paper> PaperSelectedCommand { get; set; }

        public Command RefreshCommand { get; set; }

        public Paper SelectedPaper { get; set; }

        private IJSONWebClient _client;

        public FavoritesViewModel()
        {
            _hub = DependencyService.Resolve<TinyMessengerHub>();
            _hub.Subscribe<LoggedInMessage>(OnLoggedIn);
            _hub.Subscribe<LoggedOutMessage>(OnLoggedOut);

            _client = DependencyService.Resolve<IJSONWebClient>();
            FetchPapers();

            PaperSelectedCommand = new Command<Paper>(PaperSelected);
            RefreshCommand = new Command(FetchPapers);
        }

        private void OnLoggedOut(LoggedOutMessage obj)
        {
            Papers = new ObservableCollection<Paper>();
        }

        private void OnLoggedIn(LoggedInMessage obj)
        {
            FetchPapers();
        }

        private async void PaperSelected(Paper p)
        {
            var AM = DependencyService.Resolve<AppModel>();
            AM.CurrentPaper = p;
            await Shell.Current.GoToAsync($"paper?id={p.id}");            
        }

        string _keywords = string.Empty;
        private ObservableCollection<Paper> _papers;

        internal void FilterPapers(string keywords)
        {
            _keywords = keywords;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Papers)));
        }

        public bool NeedsAuthentication
        {
            get
            {
                var AM = DependencyService.Resolve<AppModel>();
                return !AM.IsLoggedIn;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                var AM = DependencyService.Resolve<AppModel>();
                return AM.IsLoggedIn;
            }
        }

        private async void FetchPapers()
        {
            var AM = DependencyService.Resolve<AppModel>();
            if (!AM.IsLoggedIn)
            {
                await Shell.Current.GoToAsync("login");
            }
            else
            {

                await _client.OpenURL(Constants.BASE_URI + "favorite/index/?caller=wpb-iPhone", MethodEnum.GET, true);
                var papers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PaperNode>>(_client.ResponseText);
                
                AM.Favorites = new List<Paper>();
                foreach (var node in papers)
                {
                    AM.Favorites.Add(node.paper);
                }

                Barrel.Current.Add(key: nameof(AppModel), data: AM, expireIn: TimeSpan.FromDays(1));

                Papers = new ObservableCollection<Paper>(AM.Favorites);
            }
        }
    }
}

