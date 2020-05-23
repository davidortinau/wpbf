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
    public class TagsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Tag> Tags
        {
            get
            {
                if (string.IsNullOrEmpty(_keywords))
                {
                    return _tags;
                }
                else
                {
                    return new ObservableCollection<Tag>(_tags
                    .Where(x => x.name.IndexOf(_keywords, StringComparison.InvariantCultureIgnoreCase) > -1)
                    .ToList<Tag>());
                }
            }
            set
            {

                _tags = value;
            }
        }

        public Command<Tag> SelectedCommand { get; set; }

        public Tag Selected { get; set; }

        private IJSONWebClient _client;

        public TagsViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();
            Fetch();

            SelectedCommand = new Command<Tag>(TagSelected);
        }

        private async void TagSelected(Tag t)
        {
            var AM = DependencyService.Resolve<AppModel>();
            AM.CurrentTag = t;
            //await Shell.Current.Navigation.PushAsync(new TagPapersPage());
            await Shell.Current.GoToAsync($"tag/papers?id={t.id}");

        }

        string _keywords = string.Empty;
        private ObservableCollection<Tag> _tags;

        internal void FilterPapers(string keywords)
        {
            _keywords = keywords;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
        }

        private async void Fetch()
        {
            await _client.OpenURL(Constants.BASE_URI + "tag.json?caller=wpb-iPhone");
            var tags = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TagNode>>(_client.ResponseText);

            var AM = DependencyService.Resolve<AppModel>();

            AM.Tags = new List<Tag>();
            foreach (var node in tags)
            {
                AM.Tags.Add(node.tag);
            }

            Barrel.Current.Add(key: nameof(AppModel), data: AM, expireIn: TimeSpan.FromDays(365));
            
            Tags = new ObservableCollection<Tag>(AM.Tags);
        }
    }
}

