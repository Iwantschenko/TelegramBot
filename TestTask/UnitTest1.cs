using Microsoft.Extensions.Caching.Memory;
using Moq;
using BotLibrary;
using Telegram.Bot.Types;
using Telegram.Bot;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace TestTask
{
    public class Tests
    {
        private string _URL_PV = $"https://api.privatbank.ua/p24api/exchange_rates?date={DateTime.Now.ToString("dd.MM.yyy")}";
        [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public void GetRequest_Test()
        {
            GetRequest request = new GetRequest(_URL_PV);
            request.Run();

            Assert.NotNull(request.Response);
        }
        [Test]
        public void GetCurrency_CachingCurrencyTest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            string key = "usd";
            string expectedValue = "USD data from cache";

            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
            };

            cache.Set(key,expectedValue , options);

            var cachingCurrency = new CachingCurrency(cache);
            string res = cachingCurrency.GetCurrency(key);
            
            Assert.AreEqual(expectedValue,res);
        }

        

        [Test]
        public void Bot_Check_Date_True()
        {
            bool boolResult = Bot.InputDateCheck(DateTime.Now);
            Assert.IsTrue(boolResult);
        }

    }
}