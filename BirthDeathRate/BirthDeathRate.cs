using System;
using System.IO;
using System.Text.RegularExpressions;

[assembly: System.Reflection.AssemblyTitleAttribute("Birth-rate")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]

namespace ToJson
{
    static class BirthDeathRate
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(new FileStream(@"..\Indicators.csv", FileMode.Open, FileAccess.Read));
            StreamWriter write = new StreamWriter(new FileStream(@"..\Birth-rate.json", FileMode.OpenOrCreate, FileAccess.Write));         
            string[] headers = sr.ReadLine().Split(',');        //splitting the headers from CSV file
            string line;        //variable to store splitted words
            write.WriteLine("{");
            write.Write(" \"India\": [");
            Regex CSVParser = new Regex(",(?=(?:[^\"]|\"[^\"]*\")*$)");     //Regex to split commas outside of double-quotes
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    MatchColumn(write, headers, line, CSVParser);
                }       //end of while
                write.WriteLine("]"); write.WriteLine("}");
                write.Flush();
            }
            catch (Exception e)     //execption handling
            {
                Console.WriteLine("check the loops and typo error if not fixed then contact me : Kaustubh");
                Console.WriteLine(e.Message);
            }
        }

        private static void MatchColumn(StreamWriter write, string[] headers, string line, Regex CSVParser)
        {
            if (line.StartsWith("India"))
            {
                String[] val = CSVParser.Split(line);
                if (val[1] == "IND")
                {
                    if (val[2] == "\"Birth rate, crude (per 1,000 people)\"")
                    {
                        write.WriteLine(" {");
                        write.WriteLine("\"" + headers[4] + "\"" + ":" + "\"" + (val[4]) + "\"" + ",");
                        write.WriteLine("\"Birth_rate\"" + ":" + "\"" + (val[5]) + "\"" + ",");
                        write.Flush();
                    }       //end of latest if
                    else if (val[2] == "\"Death rate, crude (per 1,000 people)\"")
                    {
                        write.WriteLine("\"Death_rate\"" + ":" + "\"" + (val[5]) + "\"");
                        if (val[4] == "2013" && val[5] == "7.385") write.WriteLine("  }");
                        else write.WriteLine(" },");
                    }       //end of else if
                }       //end of nested if
            }       //end of if
        }
    }
}
