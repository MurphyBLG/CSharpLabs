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
        private readonly WebClient client = new WebClient();
        private readonly string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
        private Dictionary<string, bool> used = new Dictionary<string, bool>();
        private HashSet<string> _ignoreFiles = new HashSet<string> { ".ico", ".xml", ".png", ".css", ".jpg", ".zip", ".ppt", ".docx", ".doc" };
        public void FindThirdPartyResourceURI(string URI)
        {
            if (!used.ContainsKey(URI)) used.Add(URI, false);
            if (used[URI]) return;
            else used[URI] = true;

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
                        
            var externalRefs = (from href in hrefs
                                where !href.Islocal
                                select href.Ref).ToList();

            if (externalRefs.Count > 0) TargetFound?.Invoke(externalRefs);

            var localRefs = (from href in hrefs
                             where href.Islocal
                             select href.Ref).ToList();

            foreach(var link in localRefs)
            {
                string fileEx = Path.GetExtension((new Uri(link)).LocalPath).ToLower();
                if ((!used.ContainsKey(link) || !used[link]) && !_ignoreFiles.Contains(fileEx))
                {
                    //Console.WriteLine(link);
                    FindThirdPartyResourceURI(link);
                }
            }
        }

        private delegate void TargetFinder(List<string> targets);
        private event TargetFinder TargetFound;
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
