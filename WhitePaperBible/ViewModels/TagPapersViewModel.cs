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
    public class TagPapersViewModel : INotifyPropertyChanged
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

        public ICommand SelectedCommand { get; set; }

        public Paper Selected { get; set; }

        private IJSONWebClient _client;

        public TagPapersViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();
            Fetch();

            SelectedCommand = new Command(PaperSelected);
        }

        private async void PaperSelected()
        {
            var AM = DependencyService.Resolve<AppModel>();
            AM.CurrentPaper = Selected;
            //await App.NavigateToAsync(new PaperDetailPage() { ID = Selected.id.ToString() });

        }

        string _keywords = string.Empty;
        private ObservableCollection<Paper> _papers;
        private string tagName;

        internal void FilterPapers(string keywords)
        {
            _keywords = keywords;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Papers)));
        }

        private async void Fetch()
        {
            var AM = DependencyService.Resolve<AppModel>();

            await _client.OpenURL(Constants.BASE_URI + "papers/tagged/" + AM.CurrentTag.name + ".json?caller=wpb-iPhone");
            var paperNodes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PaperNode>>(_client.ResponseText);

            var p = new ObservableCollection<Paper>();
            foreach (var node in paperNodes)
            {
                p.Add(node.paper);
            }

            Papers = p;
        }
    }
}

