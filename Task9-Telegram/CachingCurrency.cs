using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task9_Telegram
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
                return "there is no such currency";
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
                _cache.Set(item["currency"].ToString().ToLower(), item.ToString() , options);
            }

        }
    }
}
