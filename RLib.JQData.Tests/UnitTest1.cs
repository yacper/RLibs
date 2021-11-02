using System;
using System.Threading.Tasks;
using NUnit.Framework;
using RLib.Base;

namespace RLib.JQData.Tests
{
    public class Tests
    {
         public IJQDataClient _Client;

        [SetUp]
        public void Setup()
        {
            _Client = new JQDataClient("18621301957", "Hello@123");

            bool ret = _Client.Init().Result;
            Assert.AreEqual(ret, true);
        }

        [Test]
        public async Task GetCandels()
        {
            var rtn = await  _Client.get_security_info("000001.XSHE");
        }

        [Test]
        public async Task get_query_count()
        {
            var rtn = await  _Client.get_query_count();
        }

        [Test]
        public async Task get_all_securities()
        {
            var rtn = await  _Client.get_all_securities(ECodeType.futures);
            rtn.ToExcel("D:/futures.xls");


        }

        [Test]
        public async Task get_security_info()
        {
            var rtn = await  _Client.get_security_info("000001.XSHE");

        }

        [Test]
        public async Task get_current_price()
        {
           var ret = await _Client.get_current_price(new[] {"AU9999.XSGE", "AG9999.XSGE", "000001.XSHE", "RB2201.XSGE"});
        }

        [Test]
        public async Task get_price()
        {

            var rtn5 = await  _Client.get_price("AU9999.XSGE", ETimeFrame.D1);   // 主力连续合约

            var rtn3 = await  _Client.get_price("AU2112.XSGE", ETimeFrame.D1);

            var rtn2 = await  _Client.get_price("000001.XSHE", ETimeFrame.D1);
            var rtn = await  _Client.get_price("000001.XSHE", ETimeFrame.D1);
        }

        [Test]
        public async Task get_price_period()
        {
            var rtn = await  _Client.get_price_period("RB9999.XSGE", ETimeFrame.m5, ("2019/1/1 9:00:00").ToDateTime(), ("2019/3/31 23:00:00").ToDateTime());   // 主力连续合约
            //var rtn = await  _Client.get_price_period("RB9999.XSGE", ETimeFrame.m5, DateTime.Parse("2020/7/1 9:00:00"), DateTime.Parse("2020/7/2 23:00:00"));   // 主力连续合约

            var rtn5 = await  _Client.get_price_period("AU9999.XSGE", ETimeFrame.D1, DateTime.Parse("2020/8/31 17:43:51"), DateTime.Now);   // 主力连续合约

            //var rtn3 = await  _Client.get_price_period("AU2112.XSGE", ETimeFrame.D1);

            //var rtn2 = await  _Client.get_price("000001.XSHE", ETimeFrame.D1);
            //var rtn = await  _Client.get_price("000001.XSHE", ETimeFrame.D1);
        }

        [Test]
        public async Task get_ticks()
        {
            var rtn = await  _Client.get_ticks("ZN2112.XSGE", ("2021/10/7").ToDateTime(), ("2021/10/9").ToDateTime());   // 主力连续合约
        }

        [Test]
        public async Task get_future_contracts()
        {
            var rtn2 = await  _Client.get_future_contracts("AU", DateTime.Parse("2021/8/20"));
        }

        [Test]
        public async Task get_dominant_future()
        {
            var rtn2 = await  _Client.get_dominant_future("AU" );

        }

    }
}