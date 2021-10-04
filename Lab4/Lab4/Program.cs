using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace Lab4
{
    class HTMLAnalizer
    {
        private WebClient client = new WebClient();
        private string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
        private Dictionary<string, bool> used = new Dictionary<string, bool>();
        public void FindThirdPartyResourceURI(string URI)
        {
            if (used.ContainsKey(URI))
            {
                if (!used[URI]) used[URI] = true;
                else return;
            }

            string mainPage;
            try
            {
                mainPage = client.DownloadString(URI);
            }
            catch 
            {
                return;
            }

            var hrefs = (from href in Regex.Matches(mainPage, HRefPattern).Cast<Match>()
                         let url = href.Value.Replace("href=", "").Trim('"')
                         let unLoc = url.StartsWith("https://")
                         select new
                         {
                             Ref = unLoc ? url : $"https://vuc.susu.ru/{url}",
                             Islocal = !unLoc
                         }
                         ).ToList();

            foreach (var href in hrefs)
            {
                if (!used.ContainsKey(href.Ref) || !used[href.Ref]) Console.WriteLine(href.Ref);
                if (href.Islocal)
                {
                    if (!used.ContainsKey(href.Ref)) used.Add(href.Ref, false);
                    if (!used[href.Ref]) FindThirdPartyResourceURI(href.Ref);
                }
                used[href.Ref] = true;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HTMLAnalizer fi = new HTMLAnalizer();

            fi.FindThirdPartyResourceURI("https://vuc.susu.ru/");
        }
    }
}
