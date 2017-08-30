using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

[assembly: System.Reflection.AssemblyTitleAttribute("Birth-rate")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]

namespace ToJson
{
    static class LifeXpctncy
    {
        static void Main(string[] args)
        {
            FileStream f = new FileStream(@"..\Indicators.csv", FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(f);
            FileStream fs = new FileStream(@"..\life-xpctncy1.json", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter write = new StreamWriter(fs);
            string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT", "GEO", "HKG", "IND", "IDN", "IRN",
                "IRQ", "ISR", "JPN", "JOR", "KAZ", "KWT", "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL", "PRK", "OMN", "PAK",
                "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM", "SAS", "EAS" };  // Array of ASIAN Countries
            var str = read.ReadLine();
            string space;
            float valueForAgg = 0, valueForFemaleAgg = 0;
            Dictionary<string, float> Agg_Male = new Dictionary<string, float>();
            Dictionary<string, float> Agg_Female = new Dictionary<string, float>();
            try
            {
                while ((space = read.ReadLine()) != null)
                {
                    string[] val = SplitComma(space);       //Split Comma Outside of Double Quotes
                    MatchColumn(country, ref valueForAgg, ref valueForFemaleAgg, Agg_Male, Agg_Female, val);        //Match Indicator Life expectancy at birth male/female
                }       //end of while
                int count = 0;
                write.WriteLine("[");
                foreach (KeyValuePair<string, float> entry in Agg_Male)     //Writing into JSON file
                {
                    write.WriteLine("{");
                    write.WriteLine("\"CountryCode\":" + "\"" + entry.Key + "\"" + ",");
                    write.WriteLine("\"Life_expectancy_at_birth_male\":" + entry.Value + ",");
                    write.WriteLine("\"Life_expectancy_at_birth_female\":" + Agg_Female[entry.Key]);
                    count++;
                    write.WriteLine("}");
                    if (count != Agg_Male.Count)
                    {
                        write.WriteLine(",");
                    }
                }       //end of for each
                write.WriteLine("]");
                write.Flush();
            }
            catch (Exception e)     //execption handling
            {
                Console.WriteLine("check the loops and typo error if not fixed then contact me : Kaustubh");
                Console.WriteLine(e.Message);
            }
        }

        private static string[] SplitComma(string space)
        {
            string[] val = space.Split(',');
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i].StartsWith("\""))
                {
                    if (!val[i].EndsWith("\""))
                    {
                        StringBuilder sb = new StringBuilder();
                        val[i] = sb.Append(val[i]).Append(val[i + 1]).ToString();
                        val = val.Where((value, idx) => idx != (i + 1)).ToArray();
                    }
                }       //end of if
            }       //end of for

            return val;
        }

        private static void MatchColumn(string[] country, ref float valueForAgg, ref float valueForFemaleAgg, Dictionary<string, float> Agg_Male, Dictionary<string, float> Agg_Female, string[] val)
        {
            foreach (var i in country)
            {
                if (val[1] == i)
                {
                    if (val[2] == "\"Life expectancy at birth male (years)\"")
                    {
                        valueForAgg += float.Parse(val[5]);

                        if (!Agg_Male.ContainsKey(val[1]))
                        {
                            Agg_Male.Add(val[1], float.Parse(val[5]));
                        }
                        else
                        {
                            Agg_Male[val[1]] += float.Parse(val[5]);
                        }

                    }       //end of nested if
                    else if (val[2] == "\"Life expectancy at birth female (years)\"")
                    {
                        valueForFemaleAgg += float.Parse(val[5]);
                        if (!Agg_Female.ContainsKey(val[1]))
                        {
                            Agg_Female.Add(val[1], float.Parse(val[5]));
                        }
                        else
                        {
                            Agg_Female[val[1]] += float.Parse(val[5]);
                        }
                    }       //end of else if
                }       //end of if
            }
        }
    }
}