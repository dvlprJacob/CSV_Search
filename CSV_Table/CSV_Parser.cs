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
        // Поле для хранения таблицы
        public CSV_Table Table { get; set; }

        // Конструктор с именем считываемого файла
        public CSV_Parser(string fileName)
        {
            string curPath = Directory.GetCurrentDirectory();
            using (StreamReader sr = new StreamReader(curPath + @"\" + fileName))
            {
                try
                {
                    // Считаем шапку таблицы, распарсим на названия колонок и типов значений

                    string[] tableHead = sr.ReadLine().Split(';');
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
                        string[] temp1 = line.Split(';');
                        row++;

                        List<object> temp2 = new List<object>();

                        // Добавим элемент первого столбца в промежуточный список
                        temp2.Add((object)temp1[0]);
                        for (int i = 1; i < cols; i++)
                        {
                            // Если есть пробел, удалим его
                            if (temp1[i] == " ")
                            {
                                string el = temp1[i].Remove(0, 1) as string;
                                break;
                            }
                            temp2.Add((object)temp1[i]);
                        }
                        j++;
                        values.Add(temp2);
                    }

                    // Преобразуем распарсенные строки в столбцы
                    List<Object[]> res = new List<object[]>(cols);

                    // SystemOutOfRange
                    for (int i = 0; i < row; i++)
                    {
                        res[i] = new object[row];
                        for (int k = 0; k < cols; k++)
                        {
                            res[i][k] = (object)values[i][k];
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
    }
}