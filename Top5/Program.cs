using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text.RegularExpressions;


namespace Ass
{
    class CountryData
    {
        public string country_code;
        public float value;
        public string country_name;
        public string indicatorname;
        public string year;
    }


    class Program
    {
        static void Main(string[] args)
        {                  
            string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT","GEO", "HKG", "IND", "IDN", "IRN", "IRQ","ISR", "JPN", "JOR", "KAZ", "KWT",
                "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL","PRK", "OMN", "PAK", "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM"  };
            Console.Write(country.Length);
            List<CountryData> list = new List<CountryData>();
            List<CountryData> firstlist = new List<CountryData>();           
            StreamReader reader = new StreamReader(new FileStream(@"C:\Users\Training\Downloads\CSV\Indicators.csv", FileMode.Open, FileAccess.Read));
            StreamWriter writer = new StreamWriter(new FileStream(@"C:\Users\Training\Downloads\CSV\Top5.json", FileMode.OpenOrCreate, FileAccess.Write));
            var header = reader.ReadLine().Split(',');
            while (!reader.EndOfStream)
            {
                string[] data = reader.ReadLine().Split(',');
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].StartsWith("\""))
                    {
                        if (data[i].EndsWith("\"")) { }
                        else
                        {
                            data[i] = data[i] + data[i + 1];
                            data = data.Where((val, idx) => idx != (i + 1)).ToArray();
                        }
                    }
                }
                if (data[3] == "SP.DYN.LE00.IN")
                {
                    float res;
                    float.TryParse(data[5], out res);
                    list.Add(new CountryData() { country_code = data[1], value = res, country_name = data[0] });
                }
            }
            var value3 = from m in list group m by m.country_name into t select new { countryname = t.Key, value = t.Sum(o => o.value) };
            var k = value3.OrderByDescending(m => m.value).Take(5);
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
            }
            writer.WriteLine("]");
            writer.Flush();
        }
    }
}
