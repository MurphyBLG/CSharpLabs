using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Lab3
{
    class ManagementCompany
    {
        private List<Building> buildings = new List<Building>();
        private double _AVGNumOfPeople = 0;

        public double AVGNumOfPeople
        {
            get
            {
                return _AVGNumOfPeople / buildings.Count;
            }
        }

        public void AddBuilding (Building a)
        {
            buildings.Add(a);
            _AVGNumOfPeople += a.GetAverageNumberOfPeolple();
        }

        public void SortBuildingsByAverageNumberOfPeolple()
        {
            buildings.Sort((f, s) => f.GetAverageNumberOfPeolple().CompareTo(s.GetAverageNumberOfPeolple()));
        }

        public void PrintBuildings()
        {
            foreach(Building b in buildings)
            {
                if (b is ResidentialBuilding)
                {       
                    Console.WriteLine("Count of rooms: {0}", ((ResidentialBuilding)b).CountOfRooms);
                    Console.WriteLine("Count of apatments: {0}", ((ResidentialBuilding)b).CountOfApartments);
                    Console.WriteLine("Average number of people: {0}\n", b.GetAverageNumberOfPeolple());
                }
                else
                {                    
                    Console.WriteLine("Square: {0}", ((NonResidentialBuilding)b).Square);
                    Console.WriteLine("Average number of people: {0}\n", b.GetAverageNumberOfPeolple());
                }
            }
        }

        public void PrintBuildings(int firstPos, int lastPos)
        {
            for (int i = firstPos; i < lastPos; i++)
            {
                Building exmp;
                try
                {
                    exmp = buildings[i];
                }
                catch(ArgumentOutOfRangeException)
                {
                    Console.WriteLine("ArgumentOutOfRangeException");
                    return;
                }

                if (exmp is ResidentialBuilding)
                {
                    Console.WriteLine("Count of rooms: {0}", ((ResidentialBuilding)exmp).CountOfRooms);
                    Console.WriteLine("Count of apatments: {0}", ((ResidentialBuilding)exmp).CountOfApartments);
                    Console.WriteLine("Average number of people: {0}\n", exmp.GetAverageNumberOfPeolple());
                }
                else
                {
                    Console.WriteLine("Square: {0}", ((NonResidentialBuilding)exmp).Square);
                    Console.WriteLine("Average number of people: {0}\n", exmp.GetAverageNumberOfPeolple());
                }
            }
        }
    }

    abstract class Building
    {
        public abstract double GetAverageNumberOfPeolple();
    }

    class NonResidentialBuilding : Building
    {
        public double Square { get; set; }

        public NonResidentialBuilding(double s)
        {
            Square = s;
        }

        public override double GetAverageNumberOfPeolple()
        {
            return Square * 0.2;
        }
    }

    class ResidentialBuilding : Building
    {
        public int CountOfRooms { get; set; }
        public int CountOfApartments { get; set; }

        public ResidentialBuilding(int rooms, int aparts)
        {
            CountOfRooms = rooms;
            CountOfApartments = aparts;
        }

        public override double GetAverageNumberOfPeolple()
        {
            return CountOfRooms * CountOfApartments * 1.3;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\Murphy\\Documents\\C-Labs\\Lab3\\Lab3\\Input data.xml");
            XmlNodeList residentialBuildings = doc.GetElementsByTagName("ResidentialBuilding");
            XmlNodeList nonResidentialBuildings = doc.GetElementsByTagName("NonResidentialBuilding");

            ManagementCompany myCompany = new ManagementCompany();

            for (int i = 0; i < residentialBuildings.Count; i++)
            {
                var tmp = residentialBuildings[i].FirstChild;
                myCompany.AddBuilding(new ResidentialBuilding(int.Parse(tmp.InnerText), int.Parse(tmp.NextSibling.InnerText)));
            }

            for (int i = 0; i < nonResidentialBuildings.Count; i++)
            {
                var tmp = nonResidentialBuildings[i].FirstChild;
                myCompany.AddBuilding(new NonResidentialBuilding(double.Parse(tmp.InnerText)));
            }


            myCompany.PrintBuildings();
            myCompany.SortBuildingsByAverageNumberOfPeolple();
            myCompany.PrintBuildings(1, 30);
        }
    }
}
