using Newtonsoft.Json;
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
        public string indicatorcode;
        public string year;
    }
    class Program
    {
        static void Main(string[] args)
        {
            string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT","GEO", "HKG", "IND", "IDN", "IRN", "IRQ","ISR", "JPN", "JOR", "KAZ", "KWT",
                "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL","PRK", "OMN", "PAK", "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM"  };
            Console.WriteLine(country.Length);
            List<CountryData> list = new List<CountryData>();
            List<CountryData> firstlist = new List<CountryData>();
            List<CountryData> lis = new List<CountryData>();
            StreamReader reader = new StreamReader(new FileStream(@"C:\Users\Training\Downloads\CSV\Indicators.csv", FileMode.Open, FileAccess.Read));
            StreamWriter writer = new StreamWriter(new FileStream(@"C:\Users\Training\Downloads\CSV\newton\stackedchart_new.json", FileMode.OpenOrCreate, FileAccess.Write));
            StreamWriter writer2 = new StreamWriter(new FileStream(@"C:\Users\Training\Downloads\CSV\newton\multilinechart_new.json", FileMode.OpenOrCreate, FileAccess.Write));
            StreamWriter writer3 = new StreamWriter(new FileStream(@"C:\Users\Training\Downloads\CSV\newton\barchart_new.json", FileMode.OpenOrCreate, FileAccess.Write));
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
                for (int j = 0; j < country.Length; j++)                      //for first json 
                {
                    if (country[j] == data[1])
                    {
                        if (data[3] == "SP.DYN.LE00.FE.IN" || data[3] == "SP.DYN.LE00.MA.IN")
                        {
                            float abc;
                            float.TryParse(data[5], out abc);
                            firstlist.Add(new CountryData() { country_code = data[1], indicatorname = data[2], year = data[4], value = abc });
                        }
                    }
                }
                if (data[1] == "IND")                                            //for second json
                {
                    if (data[4] == "SP.DYN.CBRT.IN" || data[4] == "SP.DYN.CDRT.IN")
                    {
                        float abc;
                        float.TryParse(data[6], out abc);

                        lis.Add(new CountryData() { year = data[5], value = abc, indicatorcode = data[4], country_code = data[1], indicatorname = data[2] });
                    }
                    writer2.Flush();
                }
                if (data[3] == "SP.DYN.LE00.IN")                                 //for third json
                {
                    float abc;
                    float.TryParse(data[5], out abc);
                    list.Add(new CountryData() { country_code = data[1], value = abc, country_name = data[0] });
                }
            }
            var val1 = from m in lis group new { m.value, m.indicatorcode } by m.year into xyz select xyz;
            writer2.WriteLine("[{");
            foreach (var i in val1)
            {
                writer2.WriteLine("\n" + "\"" + i.Key + "\"" + ":" + "{");
                foreach (var j in i)
                {
                    if (j.indicatorcode == "SP.DYN.CBRT.IN")
                        writer2.WriteLine("\"" + "Birth Value" + "\"" + ":" + "\"" + j.value + "\"" + ",");
                    else
                        writer2.WriteLine("\"" + "Death Value" + "\"" + ":" + "\"" + j.value + "\"");
                }
                writer2.WriteLine("},");
            }
            writer2.WriteLine("}]");
            writer2.Flush();
            var value4 = from m in firstlist group new { m.value, m.country_code, m.indicatorcode } by m.year into xyz select xyz;
            //  var value5=from m in value4 group 
            foreach (var j in value4)
            {
                string output = JsonConvert.SerializeObject(j.Key);
                writer.WriteLine(output);
                foreach (var i in j)
                {
                    string output2 = JsonConvert.SerializeObject(i);
                    writer.WriteLine(output2);
                }
            }
            writer.Flush();
            var value3 = from m in list group m by m.country_name into t select new { countryname = t.Key, value = t.Sum(o => o.value) };
            var k = value3.OrderByDescending(m => m.value).Take(5);
            foreach (var i in k)
            {
                string output = JsonConvert.SerializeObject(i);
                writer3.WriteLine(output);
            }
            writer3.Flush();
        }
    }
}










