using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using WhitePaperBible.Core;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using WhitePaperBible.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    [QueryProperty("ID", "id")]
    public class PaperDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ToggleFavoriteCommand { get; set; }

        public ICommand ShareCommand { get; set; }

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
            ShareCommand = new Command(async ()=> await OnShare());
            App.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            HtmlContent = DependencyService.Resolve<AppModel>().CurrentPaper.HtmlContent;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(HtmlContent)));
        }

        private async Task OnShare()
        {
            //NSString* paperTitle = [NSString stringWithFormat: @"%@",[paper valueForKey: @"title"]];
            //NSString* paperURLTitle = [NSString stringWithFormat: @"%@",[paper valueForKey: @"url_title"]];
            //NSString* subject = [NSString stringWithFormat: @"White Paper Bible: %@", paperTitle];
            //NSString* paperFullURL = [NSString stringWithFormat: @"%@/papers/%@", kSiteURL, paperURLTitle];

            await Share.RequestAsync(new ShareTextRequest(
                Paper.title,
                $"http://www.whitepaperbible.org/papers/{Paper.url_title}"
            ));
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
                if(IsFavorite)
                {
                    AM.Favorites.Remove(AM.CurrentPaper);
                }
                else
                {
                    AM.Favorites.Add(AM.CurrentPaper);
                }

                IsFavorite = !IsFavorite;
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
            Paper = AM.CurrentPaper;
        }
    }



    //public class Payload
    //{
    //    public PaperNode[] papers;
    //    public PaperHistoryNode[] popular;
    //}
}
