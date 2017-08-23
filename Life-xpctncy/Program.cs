using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

namespace temp3
{

    class Program
    {
        static void Main(string[] args)
        {
            FileStream f = new FileStream(@"C:\Users\Training\Downloads\CSV\Indicators.csv", FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(f);

            FileStream fs = new FileStream(@"C:\Users\Training\Downloads\CSV\final1.json", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter write = new StreamWriter(fs);



            string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT", "GEO", "HKG", "IND", "IDN", "IRN", "IRQ", "ISR", "JPN", "JOR", "KAZ", "KWT", "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL", "PRK", "OMN", "PAK", "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM", "SAS", "EAS" };

            var str = read.ReadLine();
            string[] words = str.Split(',');
            Stopwatch stopWatch = new Stopwatch();
            StreamReader read3 = new StreamReader(f);
            var str3 = read.ReadLine();
            string space;



            float valueForAvg = 0;

            float valueForFemaleAvg = 0;

            Dictionary<string, float> AvgForMale = new Dictionary<string, float>();

            Dictionary<string, float> AvgForFemale = new Dictionary<string, float>();


            while ((space = read.ReadLine()) != null)
            {

                string[] words3 = space.Split(',');
                for (int i = 0; i < words3.Length; i++)
                {
                    if (words3[i].StartsWith("\""))
                    {
                        if (words3[i].EndsWith("\"")) { }
                        else
                        {
                            words3[i] = words3[i] + words3[i + 1];
                            words3 = words3.Where((value, idx) => idx != (i + 1)).ToArray();
                        }
                    }
                }
                string[] words4 = words3;
                foreach (var i in country)
                {

                    if (words3[1] == i)
                    {
                        if (words3[3] == "SP.DYN.LE00.MA.IN")
                        {

                            valueForAvg += float.Parse(words3[5]);

                            if (!AvgForMale.ContainsKey(words3[1]))
                            {
                                AvgForMale.Add(words3[1], float.Parse(words3[5]));
                            }
                            else
                            {
                                AvgForMale[words3[1]] += float.Parse(words3[5]);
                            }

                        }


                        else if (words3[3] == "SP.DYN.LE00.FE.IN")
                        {

                            valueForFemaleAvg += float.Parse(words3[5]);
                            if (!AvgForFemale.ContainsKey(words3[1]))
                            {
                                AvgForFemale.Add(words3[1], float.Parse(words3[5]));
                            }
                            else
                            {
                                AvgForFemale[words3[1]] += float.Parse(words3[5]);
                            }
                        }
                    }
                }



            }
            int count = 0;

            write.WriteLine("[");
            foreach (KeyValuePair<string, float> entry in AvgForMale)
            {
                write.WriteLine("{");
                write.WriteLine("\"CountryCode\":" + "\"" + entry.Key + "\"" + ",");
                write.WriteLine("\"Life_expectancy_at_birth_male\":" + entry.Value + ",");
                write.WriteLine("\"Life_expectancy_at_birth_female\":" + AvgForFemale[entry.Key]);
                count++;
                write.WriteLine("}");
                if (count != AvgForMale.Count)
                {
                    write.WriteLine(",");
                }

            }
            write.WriteLine("]");
            write.Flush();

        }
    }
}