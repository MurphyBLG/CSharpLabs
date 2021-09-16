using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    class String
    {
        private List<char> symbols = new List<char>();
        public List<char> Symbols { get => symbols; set => symbols = value; }
        public String(List<char> vs)
        {
            this.Symbols = vs;
        }
        public char this[int idx]
        {
            get => Symbols[idx];
            set => Symbols[idx] = value;
        }
        public static String operator +(String a, char c)
        {
            a.Symbols.Add(c);
            return new String(a.Symbols);
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            String s = new String(Console.ReadLine().ToList());
            Console.WriteLine(s.Symbols.ToArray());
            s += 'a';
            Console.WriteLine(s.Symbols.ToArray());
        }
    }
}
