using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[assembly: System.Reflection.AssemblyTitleAttribute("TopFive")]
[assembly: System.Reflection.AssemblyVersion("1.0.0.0")]

namespace ToJson
{
   public static class TopFiveCountry
    {
        static void Main(string[] args)
        {
            List<Countryval> list = new List<Countryval>();
            StreamReader reader = new StreamReader(new FileStream(@"..\Indicators.csv", FileMode.Open, FileAccess.Read));
            StreamWriter writer = new StreamWriter(new FileStream(@"..\top5.json", FileMode.OpenOrCreate, FileAccess.Write));
            try
            {
                while (!reader.EndOfStream)
                {
                    string[] val = SplitComma(reader);
                    if (val[2] == "\"Life expectancy at birth total (years)\"")
                    {
                        float res;
                        float.TryParse(val[5], out res);
                        list.Add(new Countryval() { country_code = val[1], value = res, country_name = val[0] });
                    }
                }
                var value3 = from m in list group m by m.country_name into t select new { countryname = t.Key, value = t.Sum(o => o.value) };
                var k = value3.OrderByDescending(m => m.value).Take(5);     //taking top 5 countries which has Life expectancy at birth total (years)
                writer.WriteLine("[");
                foreach (var i in k)
                {
                    if (i.countryname == "Norway")
                    {
                        writer.WriteLine("{" + "\"" + "Country Name" + "\"" + ":" + "\"" + i.countryname + "\"" + "," + "\n" + "\"" + "Values" + "\"" + ":" + "\"" + i.value + "\"" + "\n" + "}");
                    }
                    else
                    {
                        writer.WriteLine("{" + "\"" + "Country Name" + "\"" + ":" + "\"" + i.countryname + "\"" + "," + "\n" + "\"" + "Values" + "\"" + ":" + "\"" + i.value + "\"" + "\n" + "}" + ",");
                    }
                }       //end of foreach
                writer.WriteLine("]");
                writer.Flush();
            }
            catch (Exception e)//execption handling
            {
                Console.WriteLine("check the loops and typo error if not fixed then contact me : Kaustubh");
                Console.WriteLine(e.Message);
            }
        }

        private static string[] SplitComma(StreamReader reader)
        {
            string[] val = reader.ReadLine().Split(',');
            for (int i = 0; i < val.Length; i++)       //To split commas outside of double-quotes
            {
                if (val[i].StartsWith("\""))
                {
                    if (!val[i].EndsWith("\""))
                    {
                        StringBuilder sb = new StringBuilder();
                        val[i] = sb.Append(val[i]).Append(val[i + 1]).ToString();
                        val = val.Where((value, idx) => idx != (i + 1)).ToArray();
                    }
                }
            }       //end of for                            

            return val;
        }
    }
}
