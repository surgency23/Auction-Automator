using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

namespace AuctionAutomation
{
    class Program
    {
        static void Main()
        {
            WebInteractions auctionPull = new WebInteractions();
            JObject selectors = auctionPull.selectorReader();
            string htmlData = auctionPull.getRequest("https://www.capitalcityonlineauction.com/cgi-bin/mncal.cgi?ccoa");
            List<AuctionObject> auctionArray = auctionPull.htmlParser(htmlData, selectors, "https://www.capitalcityonlineauction.com");
            auctionArray = auctionPull.locationBuilder(auctionArray, selectors);
            String[] terms  = new String [2]{"outdoor", "Couch"};
            auctionPull.itemSearch(auctionArray,selectors,terms);

        }
    }
}