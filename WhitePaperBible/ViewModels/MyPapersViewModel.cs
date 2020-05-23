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
    public class MyPapersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Paper> Papers
        {
            get
            {
                var AM = DependencyService.Resolve<AppModel>();
                if (!AM.IsLoggedIn)
                {
                    Shell.Current.GoToAsync("login");
                }
                else if(_papers != null)
                {
                    if (string.IsNullOrEmpty(_keywords))
                    {
                        return new ObservableCollection<Paper>(
                            _papers
                            .ToList<Paper>()
                        );
                    }
                    else
                    {
                        return new ObservableCollection<Paper>(_papers
                        .Where(x => x.title.IndexOf(_keywords, StringComparison.InvariantCultureIgnoreCase) > -1)
                        .ToList<Paper>());
                    }
                }

                return _papers;
            }
            set
            {

                _papers = value;
            }
        }

        public Command PaperSelectedCommand { get; set; }

        public Command AddPaperCommand { get; set; }

        public Command RefreshCommand { get; set; }

        public Command LogoutCommand { get; set; }

        public Paper SelectedPaper { get; set; }

        private IJSONWebClient _client;

        public MyPapersViewModel()
        {
            _hub = DependencyService.Resolve<TinyMessengerHub>();
            _hub.Subscribe<LoggedInMessage>(OnLoggedIn);
            _hub.Subscribe<LoggedOutMessage>(OnLoggedOut);

            _client = DependencyService.Resolve<IJSONWebClient>();
            FetchPapers();

            PaperSelectedCommand = new Command(PaperSelected);
            AddPaperCommand = new Command(OnAdd);
            RefreshCommand = new Command(FetchPapers);
            LogoutCommand = new Command(Logout);
        }

        private void OnLoggedOut(LoggedOutMessage obj)
        {
            Papers = new ObservableCollection<Paper>();
        }

        private void OnLoggedIn(LoggedInMessage obj)
        {
            FetchPapers();
        }

        async void Logout()
        {
            var AM = DependencyService.Resolve<AppModel>();
            AM.ClearCredentials();
            _hub.PublishAsync <LoggedOutMessage>(new LoggedOutMessage());
            await Shell.Current.GoToAsync("///papers");
        }

        private void OnAdd()
        {
            Shell.Current.Navigation.PushModalAsync(new AddPaperPage());
        }

        private async void PaperSelected()
        {
            if (SelectedPaper != null)
            {
                var AM = DependencyService.Resolve<AppModel>(); 
                AM.CurrentPaper = SelectedPaper;
                await Shell.Current.GoToAsync($"paper?id={SelectedPaper.id}");

                SelectedPaper = null;
            }

        }

        string _keywords = string.Empty;
        private ObservableCollection<Paper> _papers;
        private TinyMessengerHub _hub;

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
                //await _client.OpenURL(Constants.BASE_URI + "papers/?caller=wpb-iPhone", MethodEnum.GET, true);
                //var papers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PaperNode>>(_client.ResponseText);
                
                //AM.Papers = new List<Paper>();
                //foreach (var node in papers)
                //{
                //    AM.Papers.Add(node.paper);
                //}

                //Barrel.Current.Add(key: nameof(AppModel), data: AM, expireIn: TimeSpan.FromDays(1));

                Papers = new ObservableCollection<Paper>(AM.Papers.Where(x => x.Author.ID == AM.User.ID).ToList<Paper>());
            }
        }
    }
}

