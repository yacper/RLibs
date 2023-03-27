// created: 2022/09/06 16:42
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

using NUnit.Framework;

namespace RLib.Base.Tests.Reflection;

public class PropertyEx_Test
{
    public interface IA
    {
        int p1 { get; set; }
        
    }

    public class A:IA
    {

        public int p1 { get; set; }
        public int p2 { get; set; }
    }



    [Test]
    public void Test()
    {
        var pi = typeof(A).GetProperty(nameof(IA.p1));

    }


}