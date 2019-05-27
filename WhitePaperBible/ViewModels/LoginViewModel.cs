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
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; set; }

        public ICommand CloseModalCommand { get; set; }

        private IJSONWebClient _client;

        public LoginViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();

            LoginCommand = new Command(Login);
            CloseModalCommand = new Command(CloseModal);
        }

        private async void CloseModal()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async void Login()
        {


            //if (SelectedPaper != null)
            //{
            //    var AM = DependencyService.Resolve<AppModel>();
            //    AM.CurrentPaper = SelectedPaper;
            //    //await App.NavigateToAsync(new PaperDetailPage() { ID = SelectedPaper.id.ToString() });
            //    await Shell.Current.GoToAsync($"paper?id={SelectedPaper.id}");

            //    SelectedPaper = null;
            //}

            //await _client.OpenURL(Constants.BASE_URI + "cmd/home.json?caller=wpb-iPhone");
            //var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<Payload>(_client.ResponseText);


        }
    }



    public class Payload
    {
        public PaperNode[] papers;
        public PaperHistoryNode[] popular;
    }
}

