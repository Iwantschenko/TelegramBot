using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary
{
    public class CachingCurrency
    {
        private IMemoryCache _cache;
        public CachingCurrency(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public string GetCurrency(string key)
        {
            if (_cache.TryGetValue(key, out string value))
            {
                return value;
            }
            else
                return "there is no such currency or the date is missing ";
        }
        public void RefreshCurrency(DateTime date)
        {
            GetRequest request = new GetRequest($"https://api.privatbank.ua/p24api/exchange_rates?date={date.ToString("dd.MM.yyy")}");
            request.Run();
            var json = JObject.Parse(request.Response);
            var currencies = json["exchangeRate"];

            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            };

            foreach (var item in currencies)
            {
                string data = $"Base currency :{item["baseCurrency"]} \n" +
                              $"Currency :{item["currency"]}\n" +
                              $"Sale rate: {item["saleRateNB"]}\n" +
                              $"Purchase rate: {item["purchaseRateNB"]}\n";

                _cache.Set(item["currency"].ToString().ToLower(), data, options);
            }

        }
    }
}
