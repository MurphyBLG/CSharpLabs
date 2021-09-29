using System;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Lab4
{
    class HTMLAnalizer
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            string mainPage = client.DownloadString(new Uri("https://www.guitar-chords.org.uk"));
            File.WriteAllText(@"C:\Users\Murphy\Documents\C-Labs\Lab4\Lab4\MainPage.html", mainPage);

            string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
            var refs = Regex.Matches(mainPage, HRefPattern);

            foreach(var str in refs)
            {
                Console.WriteLine(str);
            }
        }
    }
}
