using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AuctionAutomation {
    class Program {
        static void Main() {
            WebInteractions auctionPull = new WebInteractions();
            JObject selectors = auctionPull.selectorReader();
            string htmlData = auctionPull.getRequest("https://www.capitalcityonlineauction.com");
            List < AuctionObject > auctionArray = auctionPull.htmlParser(htmlData, selectors, "https://www.capitalcityonlineauction.com");
            auctionArray = auctionPull.locationBuilder(auctionArray, selectors);

        }
    }
}