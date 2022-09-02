using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scrape_From_Console
{
    public static class ImageDownloader
    {
        public static List<string> retrieveImages(string address)
        {

            System.Net.WebClient wc = new System.Net.WebClient();
            List<string> imgList = new List<string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(wc.OpenRead(address));  //or whatever HTML file you have 
            HtmlNodeCollection imgs = doc.DocumentNode.SelectNodes("//img[@src]");
            if (imgs == null)
                return new List<string>();

            //foreach (HtmlNode imgg in imgs)
            //{
            //    if (imgg.Attributes["src"] == null)
            //        continue;
            HtmlAttribute src = imgs[0].Attributes["src"];

            imgList.Add(src.Value);
            //Do something with src.Value such as Get the image and save it to pictureBox
            Image img = GetImage(src.Value);
            //}
            return imgList;
        }

        public static Image GetImage(string url)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);

            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            Bitmap bmp = new Bitmap(responseStream);
            responseStream.Dispose();
            return bmp;
        }
    }
}
