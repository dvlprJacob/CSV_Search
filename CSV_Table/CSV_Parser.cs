using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSV
{
    // Парсер CSV - таблиц
    public class CSV_Parser
    {
        public string TableName { get; set; }

        // Поле для хранения таблицы
        public CSV_Table Table { get; set; }

        public CSV_Parser(CSV_Table table, string tableName = null)
        {
            try
            {
                if (Table != null)
                {
                    TableName = tableName;
                }
                if (tableName != null)
                    TableName = tableName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Конструктор с именем считываемого файла
        public CSV_Parser(string fileName, char separator, string tableName = null)
        {
            string curPath = Directory.GetCurrentDirectory();
            using (StreamReader sr = new StreamReader(curPath + @"\" + fileName))
            {
                try
                {
                    if (tableName != null)
                        TableName = tableName;
                    // Считаем шапку таблицы, распарсим на названия колонок и типов значений

                    string[] tableHead = sr.ReadLine().Split(separator);
                    var cols = tableHead.Count();
                    string[] typeNames = new string[cols];
                    string[] colNames = new string[cols];

                    // all correct
                    string tempStr = tableHead[0];
                    var tempStr2 = tempStr.TakeWhile(e => e != ' ');
                    colNames[0] = String.Join("", tempStr2);
                    tempStr2 = tempStr.SkipWhile(e => e != ' ');
                    typeNames[0] = String.Join("", tempStr2).Remove(0, 1); ;

                    for (int i = 1; i < cols; i++)
                    {
                        tempStr = tableHead[i].Remove(0, 1);
                        colNames[i] = String.Join("", tempStr.TakeWhile(e => e != ' '));
                        typeNames[i] = String.Join("", tempStr.SkipWhile(e => e != ' ')).Remove(0, 1); ;
                    }

                    // Распарсенные строки
                    List<List<Object>> values = new List<List<object>>();

                    string line = "";
                    int row = 0;

                    int j = 0;

                    // all correct
                    while (!sr.EndOfStream)
                    {
                        // Считаем картеж
                        line = sr.ReadLine();
                        // Разберем на колонки по символу разделителя
                        string[] temp1 = line.Split(separator);
                        row++;

                        List<object> temp2 = new List<object>();

                        // Добавим 1-й элемент первого столбца в промежуточный список
                        temp2.Add((object)temp1[0]);
                        for (int i = 1; i < cols; i++)
                        {
                            // Если есть пробел, удалим его
                            if (temp1[i][0] == ' ')
                            {
                                temp2.Add((object)temp1[i].Remove(0, 1) as string);
                            }
                            else

                                temp2.Add((object)temp1[i]);
                        }
                        j++;
                        values.Add(temp2);
                    }

                    // Преобразуем распарсенные строки в столбцы, то есть транспонируем матрицу
                    // List<List<object>> values в List<object[]>

                    List<Object[]> res = new List<object[]>(cols);

                    // all correct
                    for (int i = 0; i < cols; i++)
                    {
                        res.Add(new object[row]);
                        for (int k = 0; k < row; k++)
                        {
                            res[i][k] = (object)values[k][i];
                        }
                    }

                    this.Table = new CSV_Table(colNames, res, typeNames);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        public static void WriteToFile(CSV_Parser parser, string filename = "result.csv", string directory = "current")
        {
            if (parser.TableName != null)
                CSV_Table.WriteToFile(parser.Table, parser.TableName + ".csv", directory);
            else
                CSV_Table.WriteToFile(parser.Table, filename, directory);
        }

        public override string ToString()
        {
            if (Table != null && TableName != null)
                return TableName + Environment.NewLine + Table.ToString();
            if (Table != null)
                return Table.ToString();
            else
                return "CSV_Parser is empty";
        }
    }
}