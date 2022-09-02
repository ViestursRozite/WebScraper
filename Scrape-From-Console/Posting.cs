using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrape_From_Console
{
    public class Posting
    {
        public Posting(string postingText, string postingLink, string imgLink, string location, bool isAvailable)
        {
            PostingText = postingText;
            PostingLink = postingLink;
            ImgLink = imgLink;
            Location = location;
            IsAvailable = isAvailable;
        }
        public bool IsAvailable { get; set; }
        public string Location { get; set; }
        public string PostingText { get; set; }
        public string PostingLink { get; set; }
        public string ImgLink { get; set; }


    }
}
