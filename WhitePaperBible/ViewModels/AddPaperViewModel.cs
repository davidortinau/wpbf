using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using TinyMessenger;
using WhitePaperBible.Core;
using WhitePaperBible.Core.Models;
using WhitePaperBible.Core.Services;
using WhitePaperBible.Models;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    public class AddPaperViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AppModel AM = DependencyService.Get<AppModel>();

        public Command SaveCommand { get; set; }

        public Command CancelCommand { get; set; }
        
        public Command AddReferenceCommand { get; set; }
        public Command RemoveReferenceCommand { get; set; }

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
        private ObservableCollection<Tag> _tags;
        private ObservableCollection<Reference> _references;

        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set => _tags = value;
        }

        public ObservableCollection<Reference> References
        {
            get { return _references; }
            set { _references = value; }
        }

        public Command CompletedCommand
        {
            get;
            set;
        }

        public Command RemoveTagCommand
        {
            get;
            set;
        }

        public List<Book> Books
        {
            get { return AM.BibleBooks; }
        }

        private Book _book;
        public Book SelectedBook
        {
            get
            {
                if (_book == null)
                    _book = Books[0];

                return _book;
            }
            set
            {
                _book = value;
                
            }
        }

        public int SelectedChapter
        {
            get;
            set;
        } = 1;
        
        public int SelectedVerse
        {
            get;
            set;
        } = 1;

        public List<int> Chapters
        {
            get { return SelectedBook.GetChaptersToDisplay(); }
        }
        
        public List<int> Verses
        {
            get { return SelectedBook.GetVersesToDisplay(SelectedChapter); }
        }

        public AddPaperViewModel()
        {
            _client = DependencyService.Resolve<IJSONWebClient>();

            Paper = new Paper();
            Paper.Author = AM.User;
            CancelCommand = new Command(async ()=> await OnCancel());
            SaveCommand = new Command(async () => await OnSave());
            CompletedCommand = new Command<string>(OnTagCompleted);
            RemoveTagCommand = new Command<string>(OnRemoveTag);
            AddReferenceCommand = new Command<string>(OnAddReference);
            RemoveReferenceCommand = new Command<string>(OnRemoveReference);

            if(Paper.tags != null)
                Tags = new ObservableCollection<Tag>(Paper.tags);
            
            if(Paper.references != null)
                References = new ObservableCollection<Reference>(Paper.references);
        }

        private void OnRemoveReference(string obj)
        {
            var t = _references.Single(x => x.reference == obj);
            _references.Remove(t);
            Paper.references = _references.ToList();
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(References)));
        }

        private void OnAddReference(string obj)
        {
            if (_references == null)
                _references = new ObservableCollection<Reference>();
            
            _references.Add(new Reference
            {
                reference =  obj
            });
            Paper.references = _references.ToList();
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(References)));
        }

        private void OnRemoveTag(string tag)
        {
            var t = _tags.Single(x => x.name == tag);
            _tags.Remove(t);
            Paper.tags = _tags.ToList();
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
        }

        private void OnTagCompleted(string tag)
        {
            if (Tags == null)
                Tags = new ObservableCollection<Tag>();
            Tags.Add(new Tag(){name =  tag});
            Paper.tags = _tags.ToList();
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
        }

        private async Task OnCancel()
        {
            await Shell.Current.GoToAsync("..?msg=Canceled");   
        }

        private async Task OnSave()
        {
            IJSONWebClient _client;
            _client = DependencyService.Resolve<IJSONWebClient>();
            var url = Constants.BASE_URI;

            if (Paper.id <= 0)
            {
                // create
                url += String.Format("papers?paper[title]={0}&paper[description]={1}&user_id={2}&paper[tag_list]={3}",
                    Title, Description, AM.User.ID, Paper.TagsString());
                url += "&caller=wpb-iPhone";
                Debug.WriteLine($"URL: {url}");
                await _client.OpenURL (url, MethodEnum.POST, true);
                var payload = Newtonsoft.Json.JsonConvert.DeserializeObject<PaperNode>(_client.ResponseText);
                if (payload == null || payload.paper == null)
                    return; // bail here, throw some kind of error message

                Paper.id = payload.paper.id;
                Paper.title = payload.paper.title;
                Paper.description = payload.paper.description;
                
                // update to add references
                // url = Constants.BASE_URI;
                // url += String.Format ("papers/update/{0}?paper[title]={1}&paper[description]={2}&user_id={3}&paper[tag_list]={4}", Paper.id, Paper.title, Paper.description, AM.User.ID, Paper.TagsString());
                // url += "&caller=wpb-iPhone";
                
                // now add references
                foreach(var r in Paper.references){
                    // url += String.Format("&paper[references_attributes][{0}][id]={1}", r.id, r.id);
                    // url += String.Format("&paper[references_attributes][{0}][paper_id]={1}", r.id, Paper.id);
                    // url += String.Format("&paper[references_attributes][{0}][reference]={1}", r.id, r.reference);
                    // url += String.Format("&paper[references_attributes][{0}][_delete]={1}", r.id, r.delete);// i guess set this if to be deleted?
                    
                    var refUrl = Constants.BASE_URI + String.Format("papers/{0}/references?reference[reference]={1}&reference[paper_id]={2}", Paper.id, r.reference, Paper.id);
                    Debug.WriteLine($"URL: {refUrl}");
                    await _client.OpenURL(refUrl, MethodEnum.POST, true);
                }
            }

            // DependencyService.Get<TinyMessengerHub>().Publish(new );
            await Shell.Current.GoToAsync("..?msg=Paper%20Saved");
            // await Shell.Current.Navigation.PopModalAsync();
        }

        // internal class Payload
        // {
        //     [JsonProperty("paper")]
        //     public Paper Paper { get; set; }
        // }
    }
}
