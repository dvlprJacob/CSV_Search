using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSV;
using System.Collections.Generic;

namespace UnitTest_App
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ParseTest()
        {
            // Arrange

            var test1 = new CSV_Parser("table.csv", ';');
            //var test2 = new CSV_Parser("test.csv", ';');
            // Act

            var result1 = test1.Table.ColNames;

            string[] temp = new string[] { "Name", "Address", "BirthDate", "Age" };

            // Assert

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(result1[i], temp[i]);
            }
        }

        [TestMethod]
        public void SearchTest()
        {
            var test = new CSV_Parser("table.csv", ';');
            var res1 = test.Table.Find("Age", (object)31);
            var res2 = test.Table.Find("Address", (object)"LA");
            var rowVal = test.Table.GetColumnsFromRow(18);
            List<object[]> row = new List<object[]>();
            row.Add(rowVal);
            var head = test.Table.ColNames;
            var res3 = new CSV_Table(head, row, CSV_Table.GetColumnTypesCsvFormat(test.Table));

            Assert.AreEqual(res2, res3);
        }
    }
}