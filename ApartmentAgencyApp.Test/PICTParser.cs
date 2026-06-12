using System;
using System.Collections; 
using System.Globalization;
using System.IO;          
using NUnit.Framework;    
using ApartmentAgencyApp.Models; 

namespace ApartmentAgencyApp.Test
{
    internal class PICTParser
    {
        /* private static readonly string PictResultsPath = Path.Combine(
             AppDomain.CurrentDomain.BaseDirectory,
            "PICTResults.txt");*/
        //DODALA SAM SVOJU PUTANJU POSTO NIJEH TELO DA PRIMI 
        private static readonly string PictResultsPath = "C:\Users\it37-2024\source\repos\test-2-grupa-f-Djel4\ApartmentAgencyApp.Test\bin\Debug\PICTResults.txt";
        public static IEnumerable GetTestCases()
        {
            string[] lines = File.ReadAllLines(PictResultsPath);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('\t');
                if (parts.Length < 5)
                    continue;

                double distanceFromTheBeach = double.Parse(parts[0], CultureInfo.InvariantCulture);
                int percentOfPositiveReviews = int.Parse(parts[1]);
                ApartmentType apartmentType = (ApartmentType)Enum.Parse(typeof(ApartmentType), parts[2]);
                bool renovatedTheLastYear = bool.Parse(parts[3]);
                ApartmentRank expected = ParseExpected(parts[4]);

                yield return new TestCaseData(distanceFromTheBeach, percentOfPositiveReviews, apartmentType, renovatedTheLastYear)
                    .Returns(expected);
            }
        }

        private static ApartmentRank ParseExpected(string value)
        {
            value = value.Trim();
            return (ApartmentRank)Enum.Parse(typeof(ApartmentRank), value);
        }
    }
}