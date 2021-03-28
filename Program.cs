using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AuctionAutomation {
    class Program {
        static void Main() {
            WebInteractions auctionPull = new WebInteractions();
            JObject selectors = auctionPull.selectorReader();
            string htmlData = auctionPull.getRequest("https://www.capitalcityonlineauction.com/cgi-bin/mncal.cgi?ccoa");
            List < AuctionObject > auctionArray = auctionPull.htmlParser(htmlData, selectors, "https://www.capitalcityonlineauction.com");
            auctionArray = auctionPull.locationBuilder(auctionArray, selectors);
            auctionArray = auctionArray.FindAll(auction => !auction.Title.Contains("THIS AUCTION HAS ENDED"));
            
        }
    }
}