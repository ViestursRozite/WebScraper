// See https://aka.ms/new-console-template for more information
using Scrape_From_Console;
using System.Text;
using Microsoft.Data.Sqlite;


string tableName = "\"ifFunUWrong4\"";

Console.OutputEncoding = Encoding.UTF8;




string url = "https://atdodmantas.lv";

Console.WriteLine("Hello, World!");

var cnn = SqliteWrapper.Connect();
await SqliteWrapper.CreateTableIfNotExists(cnn, tableName);

HtmlAgilityPack.HtmlWeb holdWeb = FastWebScraper.web; 

int total = 4;
int page = 1;
string specificPage = $"/?page={page}";


for (int i = 0; i < total; i++)
{
    var firstResult = FastWebScraper.PullPage(holdWeb, url + $"/?page={page}");
    page++;
    
    foreach (var post in firstResult)
    {
        Console.WriteLine("Trying to enter a line..." + "Sucsess: " + SqliteWrapper.TryEnteringARow(cnn, post, tableName));
    }
}

//Have a looking func
//have a storing func
//Filtering func is bad
//No alerting func is implemented








var u = 5;

//Diemžēl nokavējāt. Jau atdots.