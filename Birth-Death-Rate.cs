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
            StreamWriter write = new StreamWriter(new FileStream(@"C:\Users\Training\Downloads\CSV\Birth-rate.json", FileMode.OpenOrCreate, FileAccess.Write));
           // string[] country = {"IND"};          
            string[] headers = sr.ReadLine().Split(',');        //splitting the headers from CSV file
            string line;        //variable to store splitted words
            write.WriteLine("{");
            write.Write(" \"India\": [");
            Regex CSVParser = new Regex(",(?=(?:[^\"]|\"[^\"]*\")*$)");     //Regex to split commas outside of double-quotes
            try
            {
                while ((line = sr.ReadLine()) != null)
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
                }       //end of while
                write.WriteLine("]"); write.WriteLine("}");
                write.Flush();
            }
            catch (Exception e)//execption handling
            {
                Console.WriteLine("check the loops and typo error if not fixed then contact me : Kaustubh");
                Console.WriteLine(e.Message);
            }
        }
    }
}
