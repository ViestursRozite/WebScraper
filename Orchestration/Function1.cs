using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Orchestration
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var outputs = new List<string>();

            var page = await context.CallActivityAsync<string>("Load1Page", @"https://atdodmantas.lv/");

            log.LogInformation("page is set");
             //why only base classes are accepted?  


            return outputs;
        }

        [FunctionName("PostText")]
        public static string PostText([ActivityTrigger] KeyValuePair<int, HtmlDocument> kvp)
        {
            HtmlDocument doc = kvp.Value;
            int targetDiv = kvp.Key;

            return doc.GetElementbyId("things")//select id = things
                .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]/div[1]/div[2]/p").InnerText;//text inside <p> element
        }

        [FunctionName("Process1ThingElement")]
        public static string Process1ThingElement([ActivityTrigger] HtmlNode div, ILogger log)
        {
            string output = "";

            string divAsFlatText = div.InnerHtml;


            if (divAsFlatText.Contains("Diemžēl nokavējāt"))
            {
                output = div.SelectSingleNode($"/div[2]/a[5]").OuterHtml //select <a> link to post button
                .Split(new char[] { '\"' }) //links contain a: href = "link-to-page"
                [1];//since href is written first grab element after first "

                output = output + "\n" + "Atdots";
            }
            else
            {
                output = div.SelectSingleNode($"/div[2]/a[5]").OuterHtml //select <a> link to post button
                .Split(new char[] { '\"' }) //links contain a: href = "link-to-page"
                [1];//since href is written first grab element after first "

                output = output + "\n" + "Pieejams";
            }

            return output;
        }


        [FunctionName("Load1Page")]
        public static string Load1Page([ActivityTrigger] string url, ILogger log)
        {
            log.LogInformation("Entered Load1Page, about to make new HtmlWeb");
            HtmlWeb web = new HtmlWeb();
            log.LogInformation("about to load page");
            HtmlDocument doc = web.Load(url);
            log.LogInformation("page loaded about to peel single node");


            var a = doc.GetElementbyId("things")//select id = things
                .SelectSingleNode($"//*[@id=\"things\"]/div[2]/div[2]/a[5]").OuterHtml //select <a> link to post button
                .Split(new char[] { '\"' }) //links contain a: href = "link-to-page"
                [1];//since href is written first grab element after first "
            log.LogInformation("peeled node about to return");
            log.LogInformation(a);


            return a;
            //int postsExpectedCount = 15;//15 useful elements per page, note 16 total
            //int targetDiv = 2;//1's indexed, first useful <div> is [2]
            //HtmlNode[] postsList = new HtmlNode[postsExpectedCount];

            //for (int i = 0; i < postsExpectedCount; i++)
            //{
            //    postsList[i] = (doc
            //        .GetElementbyId("things")//select things block
            //        .SelectSingleNode($"//*[@id=\"things\"]/div[{targetDiv}]")//in which select post
            //        );

            //    targetDiv++;
            //}

            //return postsList;
        }


        [FunctionName("Function1_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("Function1_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Function1", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}