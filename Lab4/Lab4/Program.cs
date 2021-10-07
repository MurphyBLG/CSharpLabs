using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Lab4
{
    class HTMLAnalizer
    {
        private readonly WebClient client = new WebClient();
        private readonly string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
        private Dictionary<Uri, bool> usedExternal = new Dictionary<Uri, bool>();
        private Dictionary<string, bool> usedLocal = new Dictionary<string, bool>();
        private HashSet<string> _ignoreFiles = new HashSet<string> { ".ico", ".xml", ".png", ".css", ".jpg",
                                                                     ".zip", ".ppt", ".docx", ".doc", ".pdf" };

        public delegate void TargetFoundEventHandler(string mainPageHTML, Uri link, string linkFatehr);
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
                                select new Uri(href.Ref)).ToList();

            if (externalRefs.Count > 0)
                foreach (var target in externalRefs)
                    OnTargetFound(mainPage, target, URI);

            var localRefs = (from href in hrefs
                             where href.Islocal
                             select href.Ref).ToList();

            foreach (var link in localRefs)
            {
                string fileEx = Path.GetExtension((new Uri(link)).LocalPath).ToLower();
                if ((!usedLocal.ContainsKey(link) || !usedLocal[link]) && !_ignoreFiles.Contains(fileEx))
                    FindThirdPartyResourceURI(link);
            }
        }

        protected virtual void OnTargetFound(string mainPage, Uri link, string linkFather)
        {
            if (TargetFound != null)
                TargetFound(mainPage, link, linkFather);
        }

        public void PrintExternalLink(string mainPageHTML, Uri link, string linkFather)
        {
            string fileEx = Path.GetExtension(link.LocalPath).ToLower();
            if (!usedExternal.ContainsKey(link))
                usedExternal.Add(link, false);

            if (!usedExternal[link] && !_ignoreFiles.Contains(fileEx))
            {
                string patern = "<a href=\"" + link.ToString() + "\">[^\\<]*</a>";
                var linkTitle = Regex.Match(mainPageHTML, patern).Value
                                .Replace("<a href=\"" + link.ToString() + "\">", "").Replace("</a>", "");

                if (linkTitle != "")
                {
                    WriteInfoToConsole(linkTitle, linkFather, GetNestingLevel(linkFather), link);
                    WriteInfoToCSV(linkTitle, linkFather, GetNestingLevel(linkFather), link);
                }

                usedExternal[link] = true;
            }
        }

        private void WriteInfoToConsole(string linkTitle, string linkFather, int nestingLevel, Uri link)
        {
            Console.WriteLine("Title of the link on the main page: {0}", linkTitle);
            Console.WriteLine("Page father URI: {0}", linkFather);
            Console.WriteLine("Nesting level: {0}", nestingLevel);
            Console.WriteLine("Target: {0}\n", link);
        }

        private string GetNormalizedURI(string URI)
        {
            if (URI[URI.Length - 1] != '/')
                URI += '/';

            return URI;
        }

        private int GetNestingLevel(string URI)
        {
            string link = GetNormalizedURI(URI);

            int nestingLevel = -3;
            foreach(char c in link)
            {
                if (c == '/')
                    nestingLevel++;
            }

            return nestingLevel;
        }

        public void WriteInfoToCSV(string linkTitle, string linkFather, int nestingLevel, Uri link)
        {
            if (new FileInfo(@"C:\Users\4795\Documents\Prog\CSharpLabs\Lab4\Lab4\info.csv").Length < 6)
                File.AppendAllText(@"C:\Users\4795\Documents\Prog\CSharpLabs\Lab4\Lab4\info.csv", "Title of the link on the main page" + ',' + "Page father URI" + ',' + "Nesting level" + ',' + "Target" + '\n');

            string data = linkTitle + ',' + linkFather + ',' + nestingLevel + ',' + link + '\n';
            File.AppendAllText(@"C:\Users\4795\Documents\Prog\CSharpLabs\Lab4\Lab4\info.csv", data);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            HTMLAnalizer fi = new HTMLAnalizer();

            fi.TargetFound += fi.PrintExternalLink;
            fi.FindThirdPartyResourceURI("https://vuc.susu.ru/");
        }
    }
}
