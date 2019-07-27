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

        public string Username { get; set; }

        public string Password { get; set; }

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
            var AM = DependencyService.Resolve<AppModel>();
            await _client.OpenURL(Constants.BASE_URI + String.Format("user_sessions/?user_session[username]={0}&user_session[password]={1}", Username, Password), MethodEnum.POST, false);
            AM.UserSessionCookie = _client.UserSessionCookie;
            if(_client.UserSessionCookie != null)
            {
                AM.StoreCredentials(Username, Password, _client.UserSessionCookie);
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                var forgot = await Shell.Current.DisplayAlert("Sorry", "Your username and/or password were not recognized.", "Reset Password", "Try Again");
                if (forgot)
                {                   
                    Device.OpenUri(new Uri("http://www.whitepaperbible.org"));
                }
            }
        }
    }



    public class Payload
    {
        public PaperNode[] papers;
        public PaperHistoryNode[] popular;
    }
}

