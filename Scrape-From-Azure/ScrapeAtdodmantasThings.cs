using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Scrape_From_Azure
{
    public class ScrapeAtdodmantasThings
    {
        [FunctionName("ScrapeAtdodmantasThings")]
        public void Run([TimerTrigger("* * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");







        }
    }
}
