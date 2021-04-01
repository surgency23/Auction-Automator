using System.Net;
using System.IO;
using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;

public class WebInteractions
{
    public string getRequest(string url)
    {
        string html = string.Empty;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.AutomaticDecompression = DecompressionMethods.GZip;
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            html = reader.ReadToEnd();
        }
        return html;
    }
    public List<AuctionObject> htmlParser(string html, JObject selectors, [Optional] string baseURL)
    {

        string auctionTitleSelctor = (string)selectors["auctionTitles"];
        string auctionLinkSelctor = (string)selectors["auctionLinks"];
        var auctionNumberRegex = new Regex(@"(?<=ccoa)\d+");
        var auctionList = new List<AuctionObject>();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        var auctionTitles = htmlDoc.DocumentNode.SelectNodes(auctionTitleSelctor);
        var auctionLinks = htmlDoc.DocumentNode.SelectNodes(auctionLinkSelctor);
        int totalLinks = htmlDoc.DocumentNode.SelectNodes(auctionTitleSelctor).Count;

        for (int i = 0; i < totalLinks; i++)
        {
            AuctionObject auction = new AuctionObject
            {
                Title = auctionTitles[i].InnerHtml,
                Link = (string)(baseURL + auctionLinks[i].Attributes["href"].Value)
                
            };
            auction.AuctionNumber = auctionNumberRegex.Match(auction.Link).ToString();
            auctionList.Add(auction);
        }
        return auctionList;
    }
    public JObject selectorReader()
    {
        string fileName = "Selectors.json";
        string path = Path.Combine(Environment.CurrentDirectory, @"References", fileName);
        var selectors = File.ReadAllText(path);
        JObject obj = JObject.Parse(selectors);
        return obj;
    }
    public List<AuctionObject> locationBuilder(List<AuctionObject> auctionObjects, JObject selectors)
    {
        string auctionLocationSelector = (string)selectors["location"];
        var tagRegex = new Regex(@"(?=<).+?(?<=>)");
        var addressRegex = new Regex(@"(?<=LOCATION.+>|\s)\d.+?(?<=(\d{5})|(OH|OHIO))");
        foreach (var auction in auctionObjects)
        {
            string html = getRequest(auction.Link);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var location = htmlDoc.DocumentNode.SelectSingleNode(auctionLocationSelector).InnerHtml;
            location = tagRegex.Replace(location, "");
            string address = addressRegex.Match(location).ToString();
            auction.Address = address;
        }
        auctionObjects = auctionObjects.FindAll(auction => !auction.Title.Contains("THIS AUCTION HAS ENDED"));
        return auctionObjects;
    }

    async public Task<List<AuctionObject>> itemSearch(List<AuctionObject> auctionObjects, JObject selectors, string[] searchTerms)
    {
        List<AuctionObject> returnObjects = new List<AuctionObject>();
        HttpClient client = new HttpClient();
        var requestObject = new Dictionary<string, string>() {
                {
                    "referrer",
                    "temp"
                }, {
                    "referrerPolicy",
                    "strict-origin-when-cross-origin"
                }, {
                    "method",
                    "POST"
                }, {
                    "mode",
                    "cors"
                }, {
                    "credentials",
                    "omit"
                }
            };
        foreach (AuctionObject auction in auctionObjects)
        {
            requestObject["referrer"] = auction.Link;
            foreach (string term in searchTerms)
            {

                requestObject["body"] = $"auction=c{auction.AuctionNumber}&keyword={term}&stype=ANY&search=Go%21";

                var content = new FormUrlEncodedContent(requestObject);

                var response = await client.PostAsync("https://www.capitalcityonlineauction.com/cgi-bin/mnlist.cgi", content);

                var responseString = await response.Content.ReadAsStringAsync();
                
            }

        }

        return auctionObjects;
    }
}