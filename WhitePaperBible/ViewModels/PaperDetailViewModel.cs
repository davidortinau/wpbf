using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using WhitePaperBible.Core;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using WhitePaperBible.Views;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    [QueryProperty("ID", "id")]
    public class PaperDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ToggleFavoriteCommand { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsOwned { get; set; }

        public Paper Paper { get; set; }

        public string Title { get; set; }

        public string HtmlContent { get; set; }

        public string ID
        {
            get => _id;
            set
            {
                _id = value;
                FetchPaper();
            }
        }

        private IJSONWebClient _client;
        private string _id;

        public PaperDetailViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();

            ToggleFavoriteCommand = new Command(ToggleFavorite);
        }

        private async void ToggleFavorite()
        {
            var AM = DependencyService.Resolve<AppModel>();
            if (!AM.IsLoggedIn)
            {
                await Shell.Current.Navigation.PushModalAsync(new LoginModalPage(), true);
            }
            else
            {

            }

        }

        private async void FetchPaper()
        {
            await _client.OpenURL(Constants.BASE_URI + "papers/" + ID + "/references.json?caller=wpb-iPhone");
            var payload = JsonConvert.DeserializeObject<List<ReferenceNode>>(_client.ResponseText);

            var references = new List<Reference>();
            foreach (var node in payload)
            {
                references.Add(node.reference);
            }

            var AM = DependencyService.Resolve<AppModel>();
            AM.CurrentPaper.references = references;

            Title = AM.CurrentPaper.title;
            HtmlContent = AM.CurrentPaper.HtmlContent;

            if (AM.Favorites != null && AM.Favorites.Count > 0)
            {
                IsFavorite = AM.Favorites.Any(paper => paper.id == AM.CurrentPaper.id);
            }

            IsOwned = (AM.IsLoggedIn) && (AM.CurrentPaper.user_id == AM.User.ID);
        }
    }



    //public class Payload
    //{
    //    public PaperNode[] papers;
    //    public PaperHistoryNode[] popular;
    //}
}
