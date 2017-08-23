using System;
using System.IO;
using System.Text.RegularExpressions;

namespace new_file
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(new FileStream(@"C:\Users\Training\Downloads\CSV\Indicators.csv", FileMode.Open, FileAccess.Read));
            StreamWriter write = new StreamWriter(new FileStream(@"C:\Users\Training\Downloads\CSV\Multi-line.json", FileMode.OpenOrCreate, FileAccess.Write));
            // string[] country = {"IND"};          
            string[] headers = sr.ReadLine().Split(',');
            string line;

            write.WriteLine("{");
            write.Write(" \"India\": [");
            Regex CSVParser = new Regex(",(?=(?:[^\"]|\"[^\"]*\")*$)");
            while ((line = sr.ReadLine()) != null)
            {

                if (line.StartsWith("India"))
                {
                    String[] val = CSVParser.Split(line); ;
                    if (val[1] == "IND")
                    {
                        if (val[2] == "\"Birth rate, crude (per 1,000 people)\"")
                        {
                            write.WriteLine(" {");
                            write.WriteLine("\"" + headers[4] + "\"" + ":" + "\"" + (val[4]) + "\"" + ",");
                            write.WriteLine("\"Birth_rate\"" + ":" + "\"" + (val[5]) + "\"" + ",");
                            write.Flush();
                        }
                        else if (val[2] == "\"Death rate, crude (per 1,000 people)\"")
                        {
                            write.WriteLine("\"Death_rate\"" + ":" + "\"" + (val[5]) + "\"");
                            if (val[4] == "2013" && val[5] == "7.385") write.WriteLine("  }");
                            else write.WriteLine(" },");
                        }
                    }
                }
            }
            write.WriteLine("]"); write.WriteLine("}");
            write.Flush();
        }
    }
}
