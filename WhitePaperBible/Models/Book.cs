using System.Collections.Generic;
using Newtonsoft.Json;

namespace WhitePaperBible.Models
{
    public class Book
    {
        [JsonProperty("id")]
        public string Name { get; set; } 

        [JsonProperty("verseCount")]
        public int VerseCount { get; set; } 

        [JsonProperty("chapters")]
        public List<int> Chapters { get; set; } 

        [JsonProperty("comment")]
        public string Comment { get; set; }

        public List<int> GetChaptersToDisplay()
        {
            var c = new List<int>();
            for (int i = 1; i <= Chapters.Count; i++)
            {
                c.Add(i);
            }

            return c;
    }
        
        public List<int> GetVersesToDisplay(int chapter)
        {
            var v = new List<int>();
            var verseCount = Chapters[chapter];
            for (int i = 1; i <= verseCount; i++)
            {
                v.Add(i);
            }

            return v;
        }
    }
}