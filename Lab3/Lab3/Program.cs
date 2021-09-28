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

        private void CreateXmlElem(Building b, XmlDocument res)
        {
            if (b is ResidentialBuilding)
            {
                XmlElement xRoot = res.DocumentElement;
                XmlElement newElement = res.CreateElement("ResidentialBuilding");
                XmlElement countOfRoomsElem = res.CreateElement("CountOfRooms");
                XmlElement countOfApartmentsElem = res.CreateElement("CountOfApartments");

                XmlText countOfRoomsText = res.CreateTextNode(((ResidentialBuilding)b).CountOfRooms.ToString());
                XmlText countOfApartmentsText = res.CreateTextNode(((ResidentialBuilding)b).CountOfApartments.ToString());

                countOfRoomsElem.AppendChild(countOfRoomsText);
                countOfApartmentsElem.AppendChild(countOfApartmentsText);

                newElement.AppendChild(countOfRoomsElem);
                newElement.AppendChild(countOfApartmentsElem);

                xRoot.AppendChild(newElement);
                res.Save("C:\\Users\\Murphy\\Documents\\C-Labs\\Lab3\\Lab3\\Output.xml");
            }
            else
            {
                XmlElement xRoot = res.DocumentElement;
                XmlElement newElement = res.CreateElement("NonResidentialBuilding");
                XmlElement squareElem = res.CreateElement("Square");

                XmlText squareText = res.CreateTextNode(((NonResidentialBuilding)b).Square.ToString());

                squareElem.AppendChild(squareText);

                newElement.AppendChild(squareElem);

                xRoot.AppendChild(newElement);
                res.Save("C:\\Users\\Murphy\\Documents\\C-Labs\\Lab3\\Lab3\\Output.xml");
            }
        }

        public void PrintBuildings()
        {
            XmlDocument res = new XmlDocument();
            res.Load("C:\\Users\\Murphy\\Documents\\C-Labs\\Lab3\\Lab3\\Output.xml");
            foreach (Building b in buildings)
            {
                CreateXmlElem(b, res);
            }
        }

        public void PrintBuildings(int firstPos, int lastPos)
        {
            XmlDocument res = new XmlDocument();
            res.Load("C:\\Users\\Murphy\\Documents\\C-Labs\\Lab3\\Lab3\\Output.xml");
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
            myCompany.PrintBuildings();
            myCompany.PrintBuildings(1, 30);
        }
    }
}
