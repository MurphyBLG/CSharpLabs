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
        private Dictionary<string, bool> usedExternal = new Dictionary<string, bool>();
        private Dictionary<string, bool> usedLocal = new Dictionary<string, bool>();
        private HashSet<string> _ignoreFiles = new HashSet<string> { ".ico", ".xml", ".png", ".css", ".jpg", ".zip", ".ppt", ".docx", ".doc", ".pdf" };

        public delegate void TargetFoundEventHandler(List<string> links);
        public event TargetFoundEventHandler TargetFound;

        public void FindThirdPartyResourceURI(string URI)
        {
            if (!usedLocal.ContainsKey(URI)) usedLocal.Add(URI, false);
            if (usedLocal[URI]) return;
            else usedLocal[URI] = true;

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

            if (externalRefs.Count > 0) OnTargetFound(externalRefs);

            var localRefs = (from href in hrefs
                             where href.Islocal
                             select href.Ref).ToList();

            foreach (var link in localRefs)
            {
                string fileEx = Path.GetExtension((new Uri(link)).LocalPath).ToLower();
                if ((!usedLocal.ContainsKey(link) || !usedLocal[link]) && !_ignoreFiles.Contains(fileEx))
                {
                    //Console.WriteLine(link);
                    FindThirdPartyResourceURI(link);
                }
            }
        }

        protected virtual void OnTargetFound(List<string> links)
        {
            if (TargetFound != null)
                TargetFound(links);
        }

        public void PrintExternalLinks(List<string> links)
        {
            foreach (var link in links)
            {
                string fileEx = Path.GetExtension((new Uri(link)).LocalPath).ToLower();
                if (!usedExternal.ContainsKey(link))
                    usedExternal.Add(link, false);

                if (!usedExternal[link] && !_ignoreFiles.Contains(fileEx))
                {
                    Console.WriteLine(link);
                    usedExternal[link] = true;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HTMLAnalizer fi = new HTMLAnalizer();

            fi.TargetFound += fi.PrintExternalLinks;
            fi.FindThirdPartyResourceURI("https://vuc.susu.ru/");
        }
    }
}
