using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV
{
    // Класс представляющий столбец в CSV_Table
    public class Column
    {
        // Массив обьектов типа ValueType
        public Object[] Values { get; set; }

        public Type ValueType { get; set; }

        public Column()
        {
        }

        public void Add(Object value)
        {
            if (Object.Equals(ValueType, value.GetType()))
            {
                var temp = Values.ToList<object>();
                temp.Add(value);
                Values = temp.ToArray<Object>();
            }
        }

        public Column(Object[] values, string typeName)
        {
            if (values == null)
                throw new ArgumentNullException("Object[] values is null");
            if (typeName == null)
                throw new ArgumentNullException("string typeNames is null");

            int count = values.Count();
            switch (typeName)
            {
                case "String":
                    ValueType = Type.GetType("System.String", true);
                    Values = values.Cast<string>().ToArray();
                    break;

                case "Date":
                    ValueType = Type.GetType("System.DateTime", true);
                    Values = new Object[count];
                    int i = 0;
                    foreach (var el in values)
                    {
                        Values[i] = el;
                        i++;
                    }
                    break;

                case "Float":
                    ValueType = Type.GetType("System.Double", true);
                    Values = new Object[count];
                    i = 0;
                    foreach (var el in values)
                    {
                        Values[i] = el;
                        i++;
                    }
                    break;

                case "Integer":
                    ValueType = Type.GetType("System.Int32", true);
                    Values = new Object[count];
                    i = 0;
                    foreach (var el in values)
                    {
                        Values[i] = el;
                        i++;
                    }
                    break;
            }
        }

        public override string ToString()
        {
            string temp = "";
            foreach (var el in Values)
            {
                temp += el.ToString() + " ";
            }
            return String.Format("{0}", temp);
        }
    }
}