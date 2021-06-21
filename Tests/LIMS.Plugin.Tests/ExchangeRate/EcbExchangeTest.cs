//using System;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace LIMS.Plugin.Tests.ExchangeRate
//{
//    [TestClass]
//    public class EcbExchangeTest
//    {

//        [TestMethod]
//        public async Task SimpleTest()
//        {
//            var exchange = new EcbExchange();
//            try
//            {
//                var result = await exchange.GetCurrencyLiveRates();
//                Assert.IsNotNull(result);
//            }
//            catch
//            {
//                Assert.IsFalse(false);
//            }
//        }
//    }
//}
