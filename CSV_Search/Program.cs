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
            CSV_Parser parser = new CSV_Parser("table.csv", ';', "TEST");

            Console.WriteLine(parser.TableName + Environment.NewLine + parser.Table);
            Console.WriteLine();

            var res = parser.Table.Find("Address", (object)"Banghok"); // res - null exeption on WriteToFile
            var res2 = parser.Table.Find("Age", (object)"31");
            var res3 = parser.Table.Find("Age", (object)31);
            var res4 = parser.Table.Find("Address", (object)"LA");

            CSV_Table.WriteToFile(res);
            Console.WriteLine(String.Format("Find result :\n{0}", res));

            var res11 = parser.Table.Find("Age", (object)31);
            var res21 = res11.Find("Address", (object)"LA");
            var rowVal = parser.Table.GetColumnsFromRow(18);
            List<object[]> row = new List<object[]>();
            for (int i = 0; i < 4; i++)
                row.Add(new object[] { rowVal[i] });
            var head = parser.Table.ColNames;
            var types = CSV_Table.GetColumnTypesCsvFormat(parser.Table);
            var res31 = new CSV_Table(head, row, types);
            CSV_Table.WriteToFile(res31, "OneRowTable");

            Console.WriteLine(res31);
            Console.ReadKey();
        }
    }
}