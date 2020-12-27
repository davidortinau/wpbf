using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(References)));
        }

        private void OnRemoveTag(string tag)
        {
            var t = _tags.Single(x => x.name == tag);
            _tags.Remove(t);
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
        }

        private void OnTagCompleted(string tag)
        {
            if (Tags == null)
                Tags = new ObservableCollection<Tag>();
            Tags.Add(new Tag(){name =  tag});
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
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
