using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    public class AddPaperViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AppModel AM = DependencyService.Get<AppModel>();

        public ICommand SaveCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsOwned { get; set; }

        public Paper Paper { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string HtmlContent { get; set; }

        public string ID
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        private IJSONWebClient _client;
        private string _id;

        public AddPaperViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();

            Paper = new Paper();
            Paper.Author = AM.User;
            CancelCommand = new Command(async ()=> await OnCancel());
            SaveCommand = new Command(async () => await OnSave());
        }

        private async Task OnCancel()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task OnSave()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

    }

}
