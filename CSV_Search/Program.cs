using CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSV_Search
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] names = { "Name String", "Age Int", "BDate DateTime" };
            Object[] Names = new object[] { "First name", "Last name", "Jahn" };
            Object[] Ages = new object[] { 14, 53, 23 };
            Object[] BDate = new object[] { "12.12.2000", "30.09.1990", "12.12.2000" };

            //
            CSV_Table t = new CSV_Table(names, new List<object[]>() { Names, Ages, BDate }, new string[] { "String", "Integer", "Date" });
            for (int i = 0; i < t.Columns[0].Values.Count(); i++)
            {
                for (int j = 0; j < t.Columns.Count(); j++)
                    Console.Write(t.Columns[j].Values[i] + "  ");
                Console.WriteLine();
            }
            if (new DateTime(2000, 12, 12) == Convert.ToDateTime(t.Columns[2].Values[0]))
                Console.WriteLine("true");
            //---------------------------------

            Console.WriteLine();
            Console.WriteLine("------------------------------------------\n\n");
            var bdate = Convert.ToDateTime(BDate[0]);

            CSV_Table T = t.Find("BDate DateTime", (object)bdate);
            Console.WriteLine("Find result on BDay");

            for (int i = 0; i < T.Columns[0].Values.Count(); i++)
            {
                for (int j = 0; j < T.Columns.Count(); j++)
                    Console.Write(T.Columns[j].Values[i] + "  ");
                Console.WriteLine();
            }
            Console.WriteLine("\nTypes\n");
            Console.Write(T.Columns[0].Values[0] + " birth ");
            Console.WriteLine(T.Columns[2].Values[0] + "\n");
            Console.Write(t.Columns[0].ValueType.ToString());
            Console.Write(T.Columns[0].ValueType.ToString());
            Console.Write(T.Columns[1].ValueType.ToString());
            Console.Write(T.Columns[2].ValueType.ToString());
            Console.WriteLine("\n------------------------------------------\n\n");
            CSV_Parser parser = new CSV_Parser("table.csv");
            //CSV_Table res = parser.Table.Find("Name", (object)"Matiew Smith");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}