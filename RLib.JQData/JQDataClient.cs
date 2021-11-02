/********************************************************************
    created:	2021/6/24 17:41:06
    author:		rush
    email:		yacper@gmail.com	
	
    purpose:
    modifiers:	Finnhub的一个连接
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RLib.Base;

namespace RLib.JQData
{
    public partial class JQDataClient : ObservableObject, IJQDataClient
    {
        public JQDataClient(string acc, string pwd, int callsPerMin = 300)
        {
            _Acc = acc;
            _Pwd = pwd;

            CallsPerMin = callsPerMin;
        }

        public string ApiKey => _ApiKey;
        public int CallsPerMin { get; set; }

        public int            CallsInScope
        {
            get { return _Calls.Count; }
        }

        protected List<DateTime> _Calls = new List<DateTime>();
        protected async Task _CheckCalls()
        {
            DateTime now = DateTime.Now;
            _Calls.Add(now);

            if (_Calls.Count < CallsPerMin)
                return;

            while (_Calls.Any())
            {
                if (now - _Calls[0] >= TimeSpan.FromMinutes(1))
                {
                    _Calls.RemoveAt(0);
                }
                else
                    break;
            }

            if (_Calls.Count >= CallsPerMin)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1) - (now - _Calls[0]));
            }
        }


        public string       LastErrMsg
        {
            get { return _LastErrMsg; }
            set
            {
                Set(nameof(LastErrMsg), ref _LastErrMsg, value);
            }
        }

        public async Task<bool>   Init()
        {
            //error:auth failed
            //认证失败

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_token",
                mob=_Acc,
                pwd=_Pwd
            }).ReceiveString();

            if (ret.Contains("error"))
            {
                LastErrMsg = ret;

                return false;
            }

            _ApiKey = ret;
            return true;
        }

        public async Task<int> get_query_count()
        {
            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_query_count",
                token=_ApiKey
            }).ReceiveString();


            return Convert.ToInt32(ret);
        }


        //获取平台支持的所有股票、基金、指数、期货信息
        //参数：
        //code: 证券类型,可选: stock, fund, index, futures, etf, lof, fja, fjb, QDII_fund, open_fund, bond_fund, stock_fund, money_market_fund, mixture_fund, options
        //date: 日期，用于获取某日期还在上市的证券信息，date为空时表示获取所有日期的标的信息
        public async Task<List<Security>> get_all_securities(ECodeType type, DateTime? date = null)
        {
            //await _CheckCalls();

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_all_securities",
                code=type.ToString(),
                token=_ApiKey
            }).ReceiveStream();


            List<Security> rtn = ret.FromCsv<Security>();

            return rtn;
        }
      
        public async Task<SecurityInfo> get_security_info(string code)
        {
            //await _CheckCalls();

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_security_info",
                code=code,
                token=_ApiKey
            }).ReceiveStream();

            SecurityInfo rtn = ret.FromCsv<SecurityInfo>().FirstOrDefault();

            return rtn;
        }

        public async Task<List<Bar>> get_price(string code, ETimeFrame timeframe = ETimeFrame.m1, int count = 5000,
            DateTime? endDate = null, DateTime? fq_ref_date = null)
        {
            //code: 证券代码
            //count: 大于0的整数，表示获取bar的条数，不能超过5000
            //unit: bar的时间单位, 支持如下周期：1m, 5m, 15m, 30m, 60m, 120m, 1d, 1w, 1M。其中m表示分钟，d表示天，w表示周，M表示月
            //end_date：查询的截止时间，默认是今天
            //fq_ref_date：复权基准日期，该参数为空时返回不复权数据
            if (endDate == null)
                endDate = DateTime.Now;

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_price",
                code=code,
                unit=timeframe.ToResolutionString(),
                count=count,
                end_date=endDate.Value.ToJqDate(),
                fq_ref_date=fq_ref_date==null?endDate.Value.ToJqDate():fq_ref_date.Value.ToJqDate(),
                token=_ApiKey
            }).ReceiveStream();

            List<Bar> rtn = ret.FromCsv<Bar>();

            return rtn;
        }
        public async Task<List<Bar>>     get_price_period(string code, ETimeFrame timeframe, DateTime date, DateTime? endDate = null, DateTime? fq_ref_date = null)
        {
            //指定开始时间date和结束时间end_date时间段，获取行情数据
            //code: 证券代码
            //unit: bar的时间单位, 支持如下周期：1m, 5m, 15m, 30m, 60m, 120m, 1d, 1w, 1M。其中m表示分钟，d表示天，w表示周，M表示月
            //date : 开始时间，不能为空，格式2018-07-03或2018-07-03 10:40:00，如果是2018-07-03则默认为2018-07-03 00:00:00
            //end_date：结束时间，不能为空，格式2018-07-03或2018-07-03 10:40:00，如果是2018-07-03则默认为2018-07-03 23:59:00
            //fq_ref_date：复权基准日期，该参数为空时返回不复权数据
            //注：当unit是1w或1M时，第一条数据是开始时间date所在的周或月的行情。当unit为分钟时，第一条数据是开始时间date所在的一个unit切片的行情。 最大获取1000个交易日数据
            if (endDate == null)
                endDate = DateTime.Now;

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_price_period",
                code=code,
                unit=timeframe.ToResolutionString(),
                date=date.ToJqDate(),
                end_date=endDate.Value.ToJqDate(),
                fq_ref_date=fq_ref_date==null?endDate.Value.ToJqDate():fq_ref_date.Value.ToJqDate(),
                token=_ApiKey
            }).ReceiveStream();

            List<Bar> rtn = ret.FromCsv<Bar>();

            return rtn;
        }

        public async Task<List<CurrentPrice>> get_current_price(IEnumerable<string> codes)
        {
            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_current_price",
                code= codes.Join(','),
                token=_ApiKey
            }).ReceiveStream();

            List<CurrentPrice> rtn = ret.FromCsv<CurrentPrice>();

            return rtn;

        }

        public async Task<List<Tick>> get_ticks(string code, DateTime? startDate = null, DateTime? endDate = null,
            int count = 5000, string fields = "None", bool skip = true, bool df = false)
        {

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_ticks",
                code = code,
                start_date = startDate.Value.ToJqDate(),
                end_date=endDate.Value.ToJqDate(),
                token = _ApiKey
            //}).ReceiveStream();
            }).ReceiveString();


            List<Tick> rtn = ret.FromCsv<Tick>();

            return rtn;

        }


        #region 期货

        //获取某期货品种在指定日期下的可交易合约标的列表
        //code: 期货合约品种，如 AG (白银)
        //date: 指定日期
        public async Task<List<string>> get_future_contracts(string code, DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Now;

            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_future_contracts",
                code=code,
                date=date.Value.ToJqDate(),
                token=_ApiKey
            }).ReceiveString();

            List<string> rtn = ret.Split('\n').ToList();

            return rtn;
        }

        //获取主力合约对应的标的
        //code: 期货合约品种，如 AG (白银)
        //date: 指定日期参数，获取历史上该日期的主力期货合约
        public async Task<string> get_dominant_future(string code)
        {
            var ret = await ApiUrl.PostJsonAsync(new
            {
                method = "get_dominant_future",
                code=code,
                token=_ApiKey
            }).ReceiveString();

            return ret;
        }


        #endregion


        protected string _ApiKey;
        protected string _Acc;
        protected string _Pwd;

        protected string _LastErrMsg;
    }
}
