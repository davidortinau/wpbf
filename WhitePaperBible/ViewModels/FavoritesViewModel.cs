using MonkeyCache.SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using WhitePaperBible.Core;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using WhitePaperBible.Views;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    public class FavoritesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public ICommand PaperSelectedCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        public Paper SelectedPaper { get; set; }

        private IJSONWebClient _client;

        public FavoritesViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();
            FetchPapers();

            PaperSelectedCommand = new Command(PaperSelected);
            RefreshCommand = new Command(FetchPapers);
        }

        private async void PaperSelected()
        {
            if (SelectedPaper != null)
            {
                var AM = DependencyService.Resolve<AppModel>();
                AM.CurrentPaper = SelectedPaper;
                //await App.NavigateToAsync(new PaperDetailPage() { ID = SelectedPaper.id.ToString() });
                await Shell.Current.GoToAsync($"paper?id={SelectedPaper.id}");

                SelectedPaper = null;
            }

        }

        string _keywords = string.Empty;
        private ObservableCollection<Paper> _papers;

        internal void FilterPapers(string keywords)
        {
            _keywords = keywords;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Papers)));
        }

        private async void FetchPapers()
        {
            var AM = DependencyService.Resolve<AppModel>();
            if (!AM.IsLoggedIn)
            {
                await Shell.Current.Navigation.PushModalAsync(new LoginModalPage(), true);
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

