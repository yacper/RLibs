// created: 2023/06/05 10:59
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace RLib.Base.Tests._3rdHelpers;

public struct AA
{
    public int    A    { get; set; } 
    public string B    { get; set; }
    public string Time { get; set; }
}

public class BBBase
{
    public DateTime Date { get; set; }
}

public class BB:BBBase
{
    public int    A { get; set; } 
    public string C { get; set; }
}



public class JsonExTest
{
    [Test]
    public void Test_Dump()
    {
        AA a = new AA() { A = 1, B = "hello" };
        a.DumpJson();
    }


 [Test]
    public void Test_2()
    {
        List<AA> aa = new()
        {
            new AA() { A = 1, B = "hello", Time="2021*01*01" },
            new AA() { A = 2, B = "hello", Time="2021*01*02" },
            new AA() { A = 3, B = "hello", Time="2021*01*03"},
        };

        var      astr = aa.ToJson();
        //List<BB> bb   = astr.ToJsonObj<List<BB>>();

        var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
        //jsonResolver.IgnoreProperty(typeof(Person), "Title");
        jsonResolver.RenameProperty(typeof(AA), "B", "D");

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.ContractResolver = jsonResolver;

        var json = JsonConvert.SerializeObject(aa, serializerSettings);

        var jsonResolver2 = new PropertyRenameAndIgnoreSerializerContractResolver();
        //jsonResolver.IgnoreProperty(typeof(Person), "Title");
        jsonResolver2.RenameProperty(typeof(BB), "C", "D");
        jsonResolver2.RenameProperty(typeof(BBBase), "Date", "Time");

        var serializerSettings2 = new JsonSerializerSettings();
        serializerSettings2.ContractResolver = jsonResolver2;
        //serializerSettings2.Converters = new List<JsonConverter>()
        //{
        //    new DateTimeFormatConverter("yyyy*MM*dd"),
        //};

        serializerSettings2.DateFormatString = "yyyy*MM*dd";

        var bb = JsonConvert.DeserializeObject<List<BB>>(json, serializerSettings2);

    }

}