// created: 2023/06/05 10:59
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using NUnit.Framework;

namespace RLib.Base.Tests._3rdHelpers;

public struct AA
{
    public int    A { get; set; } 
    public string B { get; set; }
}


public class JsonExTest
{
    [Test]
    public void Test_Dump()
    {
        AA a = new AA() { A = 1, B = "hello" };
        a.DumpJson();

    }


}