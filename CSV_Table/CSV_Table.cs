using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public CSV_Table Find(string columnName, object searchValue)
        {
            int colIndex = -1;
            this.GetColumnIndex(columnName, out colIndex);
            if (colIndex == -1)
                throw new Exception(String.Format("Column with name {0} is not exist on table.", columnName));

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
                throw new Exception("CSV.CSV_Table.Find() Empty result.");

            int cols = this.ColNames.Count();
            string[] types = new string[cols];
            for (int i = 0; i < cols; i++)
            {
                types[i] = this.Columns[i].ValueType.ToString();
            }
            return ToCsv_Table(this.ColNames, results, types);
        }

        private static CSV_Table ToCsv_Table(string[] colNames, List<object[]> tableRows, string[] typeNames)
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
            string[] typeNames = new string[Columns.Count()];
            for (int i = 0; i < Columns.Count(); i++)
            {
                typeNames[i] = Columns[i].ValueType.ToString();
            }
            return typeNames;
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
            int n = ColNames.Count();
            Object[] temp = new Object[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = Columns[i].Values[index];
            }
            return temp;
        }

        public Object[] GetColumnsFromRow(int index)
        {
            int n = ColNames.Count();
            Object[] temp = new Object[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = Columns[i].Values[index];
            }
            return temp;
        }
    }
}