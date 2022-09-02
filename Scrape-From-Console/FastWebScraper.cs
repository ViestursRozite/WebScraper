using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scrape_From_Console
{
    public static class FastWebScraper
    {
        public static HtmlWeb web = new HtmlWeb();//reads html

        public static WebClient client = new WebClient();//is capable of downloading images

        public static int ThingsCount(HtmlDocument doc)
        {
            return doc.GetElementbyId("things")
                .SelectNodes("//*[@id=\"things\"]/div").Count();
        }

        public static string PostText(HtmlDocument doc, int targetDiv)
        {
            return doc.GetElementbyId("things")//select id = things
                .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/p").InnerText;//text inside <p> element
        }

        public static string PostLink(HtmlDocument doc, int targetDiv)
        {
            return doc.GetElementbyId("things")//select id = things
                .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[2]/a[5]").OuterHtml //select <a> link to post button
                .Split(new char[] { '\"' }) //links contain a: href = "link-to-page"
                [1];//since href is written first grab element after first "
        }

        public static string ImgLink(HtmlDocument doc, int targetDiv)
        {
            string result;

            try
            {
                result = doc.GetElementbyId("things")//select id = things
                    .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/img").OuterHtml//1st img element
                    .Split(new char[] { '\"' }) //imiges are linked: href = "link-to-img"
                    [1];//since href is written first grab element after first "
            }
            catch (Exception except)
            {
                Console.WriteLine(except.Message + " FUCK!!!");
                Console.WriteLine("wrong img path, aight try other path....");

                try
                {
                    result = doc.GetElementbyId("things")//select id = things
                        .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/div[2]/div[1]/div[1]/img").OuterHtml//1st img element
                        .Split(new char[] { '\"' }) //imiges are linked: href = "link-to-img"
                        [1];//since href is written first grab element after first "
                }
                catch (Exception except2)
                {
                    Console.WriteLine(except2.Message + " OK, fuck this img ");
                    Console.WriteLine("smash in a placeholder");
                    result = "https://img.joomcdn.net/166e0a29590e23a7a4bd3e4b52554910e2f2d907_original.jpeg";
                }
            }
            return result;
        }

        public static string Location(HtmlDocument doc, int targetDiv)
        {
            

            string result;

            try
            {
                result = doc.GetElementbyId("things")//select id = things
                    .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/div/div/div").InnerText;//1st img element
            }
            catch (Exception except)
            {
                Console.WriteLine("wrong loc., try other path....");

                try
                {
                    result = doc.GetElementbyId("things")//select id = things
                        .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/div/h5").InnerText;//location tag
                }
                catch (Exception except2)
                {
                    Console.WriteLine("wrong loc. 2nd, try other path....");
                    try
                    {
                        result = doc.GetElementbyId("things")//select id = things
                        .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/div[1]/h5").InnerText;//location tag
                    }
                    catch (Exception except3)
                    {
                        try
                        {
                            result = doc.GetElementbyId("things")//select id = things
                            .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/div/div/div").InnerText;//location tag
                        }
                        catch (Exception exept3)
                        {
                            try
                            {
                                result = doc.GetElementbyId("things")//select id = things
                                .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/div[1]/h5").InnerText;//location tag
                            }
                            catch (Exception)
                            {
                                Console.WriteLine(exept3.Message + " OK, fuck this location tag");
                                Console.WriteLine("smash in a placeholder");
                                result = "fuck knows where";

                            }
                        }
                    }
                }
            }
            result = Regex.Replace(result, @"\s+", " ");

            return result;
        }

        public static List<Posting> PullPage(HtmlWeb web, string url)
        {
            var doc = web.Load(url);
            var posts = new List<Posting>();
            int thingsCount = ThingsCount(doc);

            for (int i = 0; i < thingsCount - 1; i++)//thingsCount - 1, cause div[1] has to be skipped
            {
                //i + 2  cause we start at div[2]


                string postText = PostText(doc, i + 2);
                string postLink = PostLink(doc, i + 2);
                string imgUrl = ImgLink(doc, i + 2);
                string location = Location(doc, i + 2);
                bool isAvailable = !location.Equals("Diemžēl nokavējāt. Jau atdots.");

                posts.Add(new Posting(postText, postLink, imgUrl, location, isAvailable));
            }

            //in doc look trough divs by id = things or class = things
            //count divs with class thing-block

            ////*[@id="things"]/div[1]  vienm'er j'aizlai'z duh

            //thingsCount = doc.GetElementbyId("things")
            //    .SelectNodes("//*[@id=\"things\"]/div").Count();

            //string postText = doc.GetElementbyId("things")//select id = things
            //    .SelectSingleNode("//*[@id=\"things\"]/div[2]/div[1]/div[2]/p").InnerText;//text inside p element

            //string postLink = doc.GetElementbyId("things")//select id = things
            //    .SelectSingleNode("//*[@id=\"things\"]/div[2]/div[2]/a[5]").OuterHtml //select <a> link to post button
            //    .Split(new char[] { '\"' }) //links contain a: href = "link-to-page"
            //    [1];//since href is written first grab element after first "

            //string imgUrl = doc.GetElementbyId("things")//select id = things
            //    .SelectSingleNode("//*[@id=\"things\"]/div[2]/div[1]/div[2]/img").OuterHtml//1st img element
            //    .Split(new char[] { '\"' }) //imiges are linked: href = "link-to-img"
            //    [1];//since href is written first grab element after first "

            //string location = doc.GetElementbyId("things")//select id = things
            //    .SelectSingleNode("//*[@id=\"things\"]/div[2]/div[1]/div[2]/div/h5").InnerText;//location tag

            return posts;
        }
    }
}
