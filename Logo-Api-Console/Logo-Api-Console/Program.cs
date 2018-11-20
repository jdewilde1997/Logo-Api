using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Xml;

namespace Logo_Api_Console {
    public class Program {
        public static void Main(string[] args) {

            AutoComplete();
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
            const string api = "https://autocomplete.clearbit.com/v1/companies/suggest?query=";

            Console.WriteLine("Search for a logo");
            var input = Console.ReadLine();

            var url = api + input;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = WebRequestMethods.Http.Get;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                using (var responseStream = response.GetResponseStream()) {
                    using (StreamReader sr = new StreamReader(responseStream, Encoding.UTF8)) {
                        // Break input down into a workable array
                        var result = sr.ReadToEnd();
                        var results = result.Trim('[', ']').Split('}');
                        results = results.Take(results.Count() - 1).ToArray();

                        // Deserializing json data to object
                        var js = new JavaScriptSerializer();
                        Logo[] logos = new Logo[results.Length];
                        for (int i = 0; i < results.Length; i++) {
                            if (results[i].StartsWith(",")) {
                                results[i] = results[i].Substring(1);
                            }
                            logos[i] = js.Deserialize<Logo>(results[i] + "}");
                        }

                        Console.WriteLine("----------------------------------");
                        foreach (var logo in logos) {
                            Console.WriteLine(logo.name);
                            Console.WriteLine(logo.domain);
                            Console.WriteLine(logo.logo);
                            Console.WriteLine("----------------------------------");
                        }
                    }
                }
            }
        }
    }
}
