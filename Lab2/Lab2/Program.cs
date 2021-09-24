using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    class String
    {
        private List<char> symbols = new List<char>();

        public List<char> Symbols { get => new List<char>(symbols); set => symbols = value; }

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
            var temp = a.Symbols;
            temp.Add(c);
            return new String(temp);
        }

        public static bool operator <(String a, String b)
        {
            if (a.Symbols.Count >= b.Symbols.Count)
            {
                for (int i = 0; i < b.Symbols.Count; i++)
                {
                    if (a[i] == b[i]) continue;
                    if (a[i] < b[i]) return true;
                    else break;
                }
                return false;
            } 
            else
            {
                for (int i = 0; i < a.Symbols.Count; i++)
                {
                    if (a[i] == b[i]) continue;
                    if (a[i] < b[i]) return true;
                    else break;
                }
                return true;
            }
        }

        public static bool operator >(String a, String b)
        {
            return b < a;
        }

        public static bool operator ==(String a, String b)
        {
            if (a.Symbols.Count == b.Symbols.Count)
            {
                for (int i = 0; i < b.Symbols.Count; i++)
                {
                    if (a[i] != b[i]) return false;
                }
                return true;
            }
            else return false;
        }

        public static bool operator !=(String a, String b)
        {
            return !(a == b);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            String a = new String(Console.ReadLine().ToList());
            String b = new String(Console.ReadLine().ToList());
            Console.WriteLine(a < b);
            Console.WriteLine(a > b);
            Console.WriteLine(a == b);
            Console.WriteLine(a != b);
            a += 'e';
            Console.WriteLine(a.Symbols.ToArray());
        }
    }
}
