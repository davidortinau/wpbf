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
    public class PapersViewModel : INotifyPropertyChanged
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

        public Paper SelectedPaper { get; set; }

        private IJSONWebClient _client;

        public PapersViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();
            FetchPapers();

            PaperSelectedCommand = new Command(PaperSelected);
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
            //if (!Barrel.Current.IsExpired(key: url))
            //{
            //    return Barrel.Current.Get<IEnumerable<Monkey>>(key: url);
            //}
            //else
            //{

            //}

            await _client.OpenURL(Constants.BASE_URI + "cmd/home.json?caller=wpb-iPhone");
            var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Payload>(_client.ResponseText);
            var papers = new List<PaperNode>(payload.papers);

            var AM = DependencyService.Resolve<AppModel>();

            AM.Papers = new List<Paper>();
            foreach (var node in papers)
            {
                AM.Papers.Add(node.paper);
            }

            Barrel.Current.Add(key: nameof(AppModel), data: AM, expireIn: TimeSpan.FromDays(1));

            Papers = new ObservableCollection<Paper>(AM.Papers);
        }
    }
}

