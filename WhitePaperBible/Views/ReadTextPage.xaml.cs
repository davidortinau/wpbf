using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using WhitePaperBible.Common;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    [QueryProperty("FilePath", "file")]
    public partial class ReadTextPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ReadTextPage()
        {
            BindingContext = this;

            InitializeComponent();
            
        }

        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                Color backgroundColor = (Color)App.Current.Resources["BackgroundColorLight"];
                Color textColor = (Color)App.Current.Resources["TextPrimaryColor_Light"];
                Color descriptionColor = (Color)App.Current.Resources["TabColor_Light"];
                if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
                {
                    backgroundColor = (Color)App.Current.Resources["BackgroundColorDark"];
                    textColor = (Color)App.Current.Resources["TextPrimaryColor_Dark"];
                    descriptionColor = (Color)App.Current.Resources["NeutralPrimary"];
                }

                string html = $"<meta name='viewport' content='initial-scale=1.0' /><style type='text/css'>body {{ color: {textColor.ToWebHex()}; background-color: {backgroundColor.ToWebHex()}; font-family: 'HelveticaNeue-Light', Helvetica, Arial, sans-serif; padding-bottom: 50px; }} h1, h2, h3, h4, h5, h6 {{ padding: 0px; margin: 0px; font-style: normal; font-weight: normal; }} h2 {{ font-family: 'HelveticaNeue', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: bold; margin-bottom: -10px; padding-bottom: 0px; }} h4 {{ font-size: 16px; }} p {{ font-family: Helvetica, Verdana, Arial, sans-serif; line-height:1.5; font-size: 16px; }} .esv-text {{ padding: 0 0 10px 0; }} .description {{ border-radius: 5px; background-color:{descriptionColor.ToWebHex()}; margin: 10px; padding: 8px; }}</style>";
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ReadTextPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("WhitePaperBible.Assets.WPBTermsAndConditions.txt");
                using (var reader = new System.IO.StreamReader(stream))
                {
                    _filePath = html + reader.ReadToEnd();
                }

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(FilePath)));
            }
        }
    }
}
