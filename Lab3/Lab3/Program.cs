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
            buildings.Sort((f, s) => f.GetAverageNumberOfPeolple().CompareTo(s.GetAverageNumberOfPeolple()) == 0 ? 
                                     f.Adress.CompareTo(s.Adress) : f.GetAverageNumberOfPeolple().CompareTo(s.GetAverageNumberOfPeolple()));
        }

        private void CreateXmlElem(Building b, XmlDocument res)
        {
            if (b is ResidentialBuilding)
            {
                XmlElement xRoot = res.DocumentElement;
                XmlElement newElement = res.CreateElement("ResidentialBuilding");
                XmlElement adressElem = res.CreateElement("Adress");
                XmlElement countOfRoomsElem = res.CreateElement("CountOfRooms");
                XmlElement countOfApartmentsElem = res.CreateElement("CountOfApartments");

                XmlText adressText = res.CreateTextNode(b.Adress);
                XmlText countOfRoomsText = res.CreateTextNode(((ResidentialBuilding)b).CountOfRooms.ToString());
                XmlText countOfApartmentsText = res.CreateTextNode(((ResidentialBuilding)b).CountOfApartments.ToString());

                adressElem.AppendChild(adressText);
                countOfRoomsElem.AppendChild(countOfRoomsText);
                countOfApartmentsElem.AppendChild(countOfApartmentsText);

                newElement.AppendChild(adressElem);
                newElement.AppendChild(countOfRoomsElem);
                newElement.AppendChild(countOfApartmentsElem);

                xRoot.AppendChild(newElement);
                res.Save(@"C:\Users\Murphy\Documents\C# labs\Lab3\Lab3\Output.xml");
            }
            else
            {
                XmlElement xRoot = res.DocumentElement;
                XmlElement newElement = res.CreateElement("NonResidentialBuilding");
                XmlElement adressElem = res.CreateElement("Adress");
                XmlElement squareElem = res.CreateElement("Square");
                
                XmlText adressText = res.CreateTextNode(b.Adress);
                XmlText squareText = res.CreateTextNode(((NonResidentialBuilding)b).Square.ToString());

                adressElem.AppendChild(adressText);
                squareElem.AppendChild(squareText);

                newElement.AppendChild(adressElem);
                newElement.AppendChild(squareElem);

                xRoot.AppendChild(newElement);
                res.Save(@"C:\Users\Murphy\Documents\C# labs\Lab3\Lab3\Output.xml");
            }
        }

        public void PrintBuildingsInConsole()
        {
            foreach (Building b in buildings)
            {
                if (b is ResidentialBuilding)
                    PrintResidentialBuildingInfo(b);
                else
                    PrintNonResidentialBuildingInfo(b);
            }
        }

        public void PrintBuildingsInConsole(int firstPos, int lastPos)
        {
            for (int i = firstPos; i < lastPos; i++)
            {
                Building exmp;
                try
                {
                    exmp = buildings[i];
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("ArgumentOutOfRangeException");
                    return;
                }

                if (exmp is ResidentialBuilding)
                    PrintResidentialBuildingInfo(exmp);
                else
                    PrintNonResidentialBuildingInfo(exmp);
            }
        }

        private void PrintResidentialBuildingInfo(Building b)
        {
            Console.WriteLine("ResidentialBuilding: {0}", b.Adress);
            Console.WriteLine("\tCount of rooms: {0}", ((ResidentialBuilding)b).CountOfRooms);
            Console.WriteLine("\tCount of apatments: {0}", ((ResidentialBuilding)b).CountOfApartments);
            Console.WriteLine("\tAverage number of people: {0}\n", b.GetAverageNumberOfPeolple());
        }

        private void PrintNonResidentialBuildingInfo(Building b)
        {
            Console.WriteLine("NonResidentialBuilding: {0}", b.Adress);
            Console.WriteLine("\tSquare: {0}", ((NonResidentialBuilding)b).Square);
            Console.WriteLine("\tAverage number of people: {0}\n", b.GetAverageNumberOfPeolple());
        }

        public void PrintBuildingsInXml()
        {
            XmlDocument res = new XmlDocument();
            res.Load(@"C:\Users\Murphy\Documents\C# labs\Lab3\Lab3\Output.xml");
            foreach (Building b in buildings)
            {
                CreateXmlElem(b, res);
            }
        }

        public void PrintBuildingsInXml(int firstPos, int lastPos)
        {
            XmlDocument res = new XmlDocument();
            res.Load(@"C:\Users\Murphy\Documents\C# labs\Lab3\Lab3\Output.xml");
            for (int i = firstPos; i < lastPos; i++)
            {
                Building exmp;
                try
                {
                    exmp = buildings[i];
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("ArgumentOutOfRangeException");
                    return;
                }

                CreateXmlElem(buildings[i], res);
            }
        }
    }

    abstract class Building
    {
        private string adress;

        public string Adress { get => adress; set => adress = value; }

        public abstract double GetAverageNumberOfPeolple();
    }

    class NonResidentialBuilding : Building
    {
        public double Square { get; set; }

        public NonResidentialBuilding(string adress, double s)
        {
            Adress = adress;
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

        public ResidentialBuilding(string adress, int rooms, int aparts)
        {
            Adress = adress;
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
            doc.Load(@"C:\Users\Murphy\Documents\C# labs\Lab3\Lab3\Input data.xml");
            XmlNodeList residentialBuildings = doc.GetElementsByTagName("ResidentialBuilding");
            XmlNodeList nonResidentialBuildings = doc.GetElementsByTagName("NonResidentialBuilding");

            ManagementCompany myCompany = new ManagementCompany();

            for (int i = 0; i < residentialBuildings.Count; i++)
            {
                var tmp = residentialBuildings[i].FirstChild;
                myCompany.AddBuilding(new ResidentialBuilding(tmp.InnerText, int.Parse(tmp.NextSibling.InnerText), int.Parse(tmp.NextSibling.InnerText)));
            }

            for (int i = 0; i < nonResidentialBuildings.Count; i++)
            {
                var tmp = nonResidentialBuildings[i].FirstChild;
                myCompany.AddBuilding(new NonResidentialBuilding(tmp.InnerText, double.Parse(tmp.NextSibling.InnerText)));
            }

            myCompany.PrintBuildingsInConsole();
            myCompany.SortBuildingsByAverageNumberOfPeolple();
            myCompany.PrintBuildingsInConsole();
        }
    }
}
