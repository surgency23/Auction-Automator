using System;
using System.Net;
using System.IO;

namespace Test {
    class Program {
        static void Main(string[] args) {
            string html = string.Empty;
            string url = @"https://www.capitalcityonlineauction.com/cgi-bin/mncal.cgi?ccoa";

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using(HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            using(Stream stream = response.GetResponseStream())
            using(StreamReader reader = new StreamReader(stream)) {

                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
        }
    }
}