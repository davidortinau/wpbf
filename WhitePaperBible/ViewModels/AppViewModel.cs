using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using WhitePaperBible.Core.Models;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AppModel AM = DependencyService.Get<AppModel>();

        public Command EditImageCommand { get; set; }

        public AppViewModel()
        {
            var userHash = CalculateMD5Hash("dave@ortinau.com").ToLower();
            UserImage = $"http://gravatar.com/avatar/{userHash}?s=80&r=PG";

            EditImageCommand = new Command(OnEditImage);
        }

        private async void OnEditImage()
        {
            if(!AM.IsLoggedIn)
            {
                await Shell.Current.GoToAsync("login");
            }
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private string _userImage;
        public string UserImage { get => _userImage; set => _userImage = value; }

        
    }
}

