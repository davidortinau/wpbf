using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using WhitePaperBible.Models;

namespace WhitePaperBible.Core.Services
{
    public static class LoadBooksService
    {
        public static List<Book> Load()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadBooksService)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("WhitePaperBible.Assets.books.json");
            string text = "";
            using (var reader = new System.IO.StreamReader (stream))
            {  
                text = reader.ReadToEnd ();

                var booksOfTheBible = JsonConvert.DeserializeObject<List<Book>>(text);
                return booksOfTheBible;
            }
        }
    }
}