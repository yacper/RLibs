using System;
using NUnit.Framework;

namespace RLib.Fin.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Tick t = new Tick()
                { Time = DateTime.Now, AskLevels = new Level[1] { new Level() { Price = 1, Volume = 2, Orders = 3 } } };



            Assert.Pass();
        }
    }
}