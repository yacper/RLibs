// created: 2023/05/09 22:54
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Excel.EPPlus;
using FluentAssertions;
using NUnit.Framework;
using RLib.Base;

namespace RLib.Base.Tests;

public class CsvEx_Test
{
    [Test]
    public void Test_ReadCsv()
    {
        string csvPath = "Data/MSFT.csv";
        var r = csvPath.FromCsv();
        r.Count().Should().Be(252);

    }
    
    [Test]
    public void Test_ReadExcel()
    {

        string csvPath = "Data/MSFT.xlsx";
        //string csvPath = "Data/a.xlsx";

        //using (var reader = new CsvReader(new ExcelParser(File.Open(csvPath, FileMode.Open))))
        //{
        //    var people = reader.GetRecords<dynamic>();
        //}

        var r = csvPath.FromCsv();
        r.Count().Should().Be(252);

    }
 
}