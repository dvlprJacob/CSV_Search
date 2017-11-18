using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSV
{
    // Класс представляющий CSV - таблицу
    public class CSV_Table
    {
        // Названия столбцов
        public string[] ColNames { get; set; }

        // Массив столбцов разного типа
        public Column[] Columns { get; set; }

        public CSV_Table()
        {
        }

        public CSV_Table(string[] colNames, List<Object[]> columnValues, string[] typeNames)
        {
            try
            {
                if (columnValues == null || colNames == null || typeNames == null)
                    throw new ArgumentNullException();
                ColNames = colNames;
                Columns = new Column[columnValues.Count()];
                int i = 0;
                foreach (var column in columnValues)
                {
                    Columns[i] = new Column(column, typeNames[i]);
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + String.Format(" on CSV_Table(args)"));
            }
        }

        /// <summary>
        /// Метод возвращает результат поиска по столбцу columnName значения searchValue в виде таблицы
        /// </summary>
        /// <param name="columnName">Имя столбца</param>
        /// <param name="searchValue">Значение для поиска</param>
        /// <returns>Результат поиска, null если результат пустой или данного столбца нет в таблице</returns>
        public CSV_Table Find(string columnName, object searchValue)
        {
            try
            {
                int colIndex = -1;
                this.GetColumnIndex(columnName, out colIndex);
                if (colIndex == -1)
                    return null;

                List<object[]> results = new List<object[]>();
                int rows = this.Columns[0].Values.Count();

                for (int i = 0; i < rows; i++)
                {
                    var temp = Convert.ChangeType(this.Columns[colIndex].Values[i], this.Columns[colIndex].ValueType);

                    if (Object.Equals(temp, searchValue))
                    {
                        results.Add(this.GetRow(i));
                    }
                }

                if (results.Count() == 0)
                    return null;

                int cols = this.ColNames.Count();
                string[] types = new string[cols];
                for (int i = 0; i < cols; i++)
                {
                    types[i] = this.Columns[i].ValueType.ToString();
                }
                return ToCsv_Table(this.ColNames, results, types);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public CSV_Table Find(string[] columnNames, object[] searchValues)
        {
            if (Comparer.Equals(columnNames, null) || Comparer.Equals(searchValues, null))
                throw new ArgumentNullException();
            try
            {
                int colCount = columnNames.Count();
                int[] colIndexes = new int[colCount];
                for (int i = 0; i < colCount; i++)
                {
                    this.GetColumnIndex(columnNames[i], out colIndexes[i]);
                    if (colIndexes[i] == -1)
                        return null;
                }

                List<object[]> results = new List<object[]>();
                int rows = this.Columns[0].Values.Count();

                // Добавим результат по первому искомому столбцу
                for (int i = 0; i < rows; i++)
                {
                    var temp = Convert.ChangeType(this.Columns[colIndexes[0]].Values[i], this.Columns[colIndexes[0]].ValueType);

                    if (Object.Equals(temp, searchValues[0]))
                    {
                        results.Add(this.GetRow(i));
                    }
                }

                // Будем выкидывать строку если значения последующих после первого столбца не удовлетворяют искомым значениям
                for (int i = 1; i < colCount; i++)
                {
                    for (int j = 0; j < results.Count(); j++)
                    {
                        var temp = Convert.ChangeType(results[j][colIndexes[i]], this.Columns[colIndexes[i]].ValueType);

                        var eq = Comparer.Equals(temp, searchValues[i]);
                        if (!eq)
                        {
                            results.RemoveAt(j);
                            j--;
                        }
                    }
                }

                if (results.Count() == 0)
                    return null;

                int cols = this.ColNames.Count();
                string[] types = new string[cols];
                for (int i = 0; i < cols; i++)
                {
                    types[i] = this.Columns[i].ValueType.ToString();
                }
                return ToCsv_Table(this.ColNames, results, types);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static CSV_Table ToCsv_Table(string[] colNames, List<object[]> tableRows, string[] typeNames)
        {
            try
            {
                var T = new CSV_Table();
                T.ColNames = colNames;
                int cols = colNames.Count();
                T.Columns = new Column[cols];
                int r = tableRows.Count();

                for (int i = 0; i < cols; i++)
                {
                    T.Columns[i] = new Column();
                    T.Columns[i].Values = new Object[r];

                    for (int j = 0; j < r; j++)
                    {
                        T.Columns[i].Values[j] = tableRows.ElementAt<object[]>(j)[i];
                    }

                    T.Columns[i].ValueType = Type.GetType(typeNames[i], true);
                }
                return T;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void GetColumnIndex(string colName, out int index)
        {
            int count = ColNames.Count();
            for (int i = 0; i < count; i++)
            {
                if (ColNames[i] == colName)
                {
                    index = i;
                    return;
                }
            }
            index = -1;
            return;
        }

        public string[] GetColumnTypes()
        {
            try
            {
                string[] typeNames = new string[Columns.Count()];
                for (int i = 0; i < Columns.Count(); i++)
                {
                    typeNames[i] = Columns[i].ValueType.ToString();
                }
                return typeNames;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool ColumnIsExist(string columnName)
        {
            try
            {
                int count = ColNames.Count();
                for (int i = 0; i < count; i++)
                {
                    if (ColNames[i] == columnName)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Object[] GetRow(int index)
        {
            try
            {
                int n = ColNames.Count();
                Object[] temp = new Object[n];
                for (int i = 0; i < n; i++)
                {
                    temp[i] = Columns[i].Values[index];
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Object[] GetColumnsFromRow(int index)
        {
            try
            {
                int n = ColNames.Count();
                Object[] temp = new Object[n];
                for (int i = 0; i < n; i++)
                {
                    temp[i] = Columns[i].Values[index];
                }
                return temp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public override string ToString()
        {
            string temporary = "";
            for (int i = 0; i < ColNames.Length - 1; i++)
            {
                var type = Column.GetColumnTypeCsvFormat(Columns[i]);
                temporary += ColNames[i] + " " + type + "; ";
            }
            temporary += ColNames.Last() + " " + Column.GetColumnTypeCsvFormat(Columns.Last()) + ";" + Environment.NewLine;
            for (int i = 0; i < Columns[0].Values.Length; i++)
            {
                var row = this.GetColumnsFromRow(i);
                for (int j = 0; j < row.Length - 1; j++)
                {
                    temporary += row[j] + "; ";
                }
                temporary += row.Last() + ";" + Environment.NewLine;
            }
            return temporary;
        }

        public static string[] GetColumnTypesCsvFormat(CSV_Table table)
        {
            if (table.Columns == null)
                throw new ArgumentNullException("table.Columns is null");
            List<string> types = new List<string>();
            try
            {
                foreach (var column in table.Columns)
                {
                    types.Add(Column.GetColumnTypeCsvFormat(column));
                }
                return types.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "on static method CSV_Table.GetColumnTypesCsvFormat(CSV_Table table)");
                return null;
            }
        }

        /// <summary>
        /// Статический метод класса записывает
        /// представление таблицы в файл с расширеием .csv
        /// </summary>
        /// <param name="filename"> Имя файла, по умолчанию "result", либо TableName</param>
        /// <param name="directory"> Директория для сохранения, по умолчанию директория программы.</param>
        public static void WriteToFile(CSV_Table table, string filename = "result.csv", string directory = "current")
        {
            if (Comparer.Equals(table, null))
            {
                throw new ArgumentNullException("table is null");
            }
            if (!filename.EndsWith(".csv"))
                filename += ".csv";
            try
            {
                string path = "";
                if (directory == "current" && !string.IsNullOrEmpty(filename) && filename.EndsWith(".csv"))
                {
                    path = Directory.GetCurrentDirectory() + @"\" + filename;
                }
                else if (Directory.Exists(directory) && !string.IsNullOrEmpty(filename) && filename.EndsWith(".csv"))
                {
                    if (directory.EndsWith(@"\"))
                        path = directory + filename;
                    else
                        path = directory + @"\" + filename;
                }
                else
                {
                    throw new ArgumentException();
                }
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    Regex reg = new Regex(@".\w*$");
                    int col = table.ColNames.Count();
                    int row = table.Columns[0].Values.Count();
                    string[] colTypes = CSV_Table.GetColumnTypesCsvFormat(table);

                    // write table head { ColumnName ColumnType; ...}
                    for (int i = 0; i < col - 1; i++)
                    {
                        sw.Write(string.Format("{0} {1}; ", table.ColNames[i], colTypes[i]));
                    }
                    sw.Write(string.Format("{0} {1}{2}", table.ColNames[col - 1], colTypes[col - 1], Environment.NewLine));

                    // write rows
                    for (int i = 0; i < row; i++)
                    {
                        var curRow = table.GetRow(i);
                        for (int j = 0; j < col - 1; j++)
                        {
                            sw.Write(string.Format("{0}; ", curRow[j]));
                        }
                        sw.Write(string.Format("{0}{1}", curRow[col - 1], Environment.NewLine));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "on static method CSV_Table.WriteToFile(args)");
            }
        }

        public static CSV_Table ConcatOnColumns(CSV_Table f, CSV_Table s)
        {
            if (!Comparer.ReferenceEquals(f.ColNames, s.ColNames))
                return null;
            return new CSV_Table();
        }
    }
}