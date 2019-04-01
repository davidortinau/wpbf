using System.ComponentModel;
using System.Windows.Input;
using WhitePaperBible.Core.Services;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    [QueryProperty("ID", "id")]
    public class PaperDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PaperSelectedCommand { get; set; }

        public string ID
        {
            get;
            set;
        }

        private IJSONWebClient _client;

        public PaperDetailViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();
            //FetchPaper();

            //PaperSelectedCommand = new Command<Paper>(PaperSelected);
        }

        //private async void PaperSelected(Paper p)
        //{
        //    await Shell.CurrentShell.GoToAsync($"./papers/paper_detail?id={p.id}");
        //}

        //private async void FetchPaper()
        //{
        //    await _client.OpenURL(Constants.BASE_URI + "cmd/home.json?caller=wpb-iPhone");
        //    var payload = JsonConvert.DeserializeObject<Payload>(_client.ResponseText);
        //    var papers = new List<PaperNode>(payload.papers);

        //    var AM = DependencyService.Resolve<AppModel>();

        //    var references = new List<Reference>();
        //    foreach (var node in (args as GetPaperReferencesServiceEventArgs).References)
        //    {
        //        references.Add(node.reference);
        //    }
        //    AM.CurrentPaper.references = references;

        //    Papers = new ObservableCollection<Paper>(AM.Papers);
        //}
    }

    //public class Payload
    //{
    //    public PaperNode[] papers;
    //    public PaperHistoryNode[] popular;
    //}
}
