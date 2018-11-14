using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logo_Api_Console {
    public class Program {
        public static void Main(string[] args) {

            GetLogo();


        }

        public static void GetLogo() {

            var html = string.Empty;
            const string api = "https://logo.clearbit.com/";

            Console.WriteLine("Which logo would you like to download?");
            var input = Console.ReadLine();

            var url = api + input;

            var c = new WebClient();
            var bytes = c.DownloadData(url);
            var ms = new MemoryStream(bytes);

            File.WriteAllBytes($"{input}.png", ms.ToArray());
        }

        public static void AutoComplete() {
            const string uri = "https://autocomplete.clearbit.com/v1/companies/suggest?query=playstation";
        }
        
    }
}
