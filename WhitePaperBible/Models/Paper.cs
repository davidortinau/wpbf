using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using WhitePaperBible.Core.Helpers;
using Xamarin.Forms;
using System.Diagnostics;
using WhitePaperBible.Common;

namespace WhitePaperBible.Core.Models
{
    [DataContract(Name = "paper")]
    public class Paper
    {
        [DataMember(Name = "permalink")]
        public string permalink { get; set; }

        [DataMember(Name = "url_title")]
        public string url_title { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime updated_at { get; set; }

        [DataMember(Name = "title")]
        public string title { get; set; }
        //public bool public { get; set; }

        [DataMember(Name = "featured")]
        public bool? featured { get; set; }

        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "description")]
        public string description { get; set; }

        [DataMember(Name = "user_id")]
        public int user_id { get; set; }

        [DataMember(Name = "user")]
        public AppUser Author { get; set; }

        [DataMember(Name = "view_count")]
        public int view_count { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime created_at { get; set; }

        private List<Reference> _references;
        public List<Reference> references
        {
            get
            {
                return _references;
            }
            set
            {
                _references = value;
                //HtmlContent = generateHtmlContent();
            }
        }

        private string _htmlContent;
        public string HtmlContent
        {
            get
            {
                return _htmlContent = generateHtmlContent();
            }
            set
            {
                _htmlContent = value;
            }
        }

        public List<Tag> tags;

        public string ToPlainText()
        {
            var txt = "";
            //			txt +=	title + Environment.NewLine;
            txt += description + Environment.NewLine;

            if (references != null)
            {
                foreach (var reference in references)
                {
                    txt += HtmlToText.ConvertHtml(reference.content) + Environment.NewLine;
                }
            }

            return txt;
        }

        [DataMember(Name = "WatchText")]
        public string WatchText { get; set; }

        private string generateHtmlContent()
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
            html += "<h1>" + title + "</h1>";

            Debug.WriteLine(html);

            string author = (Author != null) ? Author.Name : "Anonymous";

            html += "<section class=\"description\">" + description + "<br><b>By " + author + "</b></section>";
            if (references != null)
            {
                foreach (var reference in references)
                {
                    html += reference.content;
                }
            }
            return html;
        }

        public void UpdateReference(Reference reference)
        {
            var exists = references.Single(x => x.reference == reference.reference);
            var atIndex = 0;
            if (exists != null)
            {
                atIndex = references.IndexOf(exists);
                references.Remove(exists);
            }

            references.Insert(atIndex, reference);
        }

        public string TagsString()
        {
            if (tags != null)
            {
                string[] tagsArray = tags.Select(x => x.name).ToArray();
                return string.Join(",", tagsArray);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
