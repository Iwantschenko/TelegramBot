using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task9_Telegram
{
   
    public class Bot 
    {
        private bool IsBoardCreate = false;
        private ResourceManager _resource;
        private CachingCurrency _currencyCache;

        public Bot(string path )
        {
            _resource = new ResourceManager("Task9_Telegram.Resource" , Assembly.GetExecutingAssembly());
            var cache = new MemoryCache(new MemoryCacheOptions());
            _currencyCache = new CachingCurrency(cache);
            TelegramBotClient client = new TelegramBotClient(path);
            client.StartReceiving(Update, Error);
           

        }
        private bool CheckCurrencyMessage(string line , out string result)
        {
            string keyWord = "key";
            
            int index = line.IndexOf(keyWord);
            if (index != -1)
            {
               result = line.Substring(index + keyWord.Length);
                return true;
            }
            else
            {
                result = string.Empty;
                return false;
            }
        }
        async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (!IsBoardCreate)
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "/Help", "/EnterDate", "/ShowCurrency" }, })
                {
                    ResizeKeyboard = true
                };
                Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Оберіть дії",
                                replyMarkup: replyKeyboardMarkup,
                                cancellationToken: cancellationToken);
                IsBoardCreate = true;
            }
            if (message.Text != null)
            {
                message.Text = message.Text.ToLower();
                message.Text = message.Text.Replace(" ", string.Empty);
                if (CheckCurrencyMessage(message.Text , out string key ))
                {
                    Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: _currencyCache.GetCurrency(key),
                                cancellationToken: cancellationToken) ;
                }
                if (DateTime.TryParseExact(message.Text ,"dd.MM.yyyy",null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    _currencyCache.RefreshCurrency(date);
                    Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"{_resource.GetString("ChangeDate")} {date})",
                                cancellationToken: cancellationToken);
                }

                switch (message.Text)
                {
                    case "/help":
                        {
                       
                            Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: _resource.GetString("HelpMessage"),
                                cancellationToken: cancellationToken);
                        }
                        break;
                    case "/enterdate":
                        {
                            Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: _resource.GetString("EnterDateMessage"),
                                cancellationToken: cancellationToken);
                        }
                        break;
                    case "/showcurrency":
                        {
                            //show currency
                        }
                        break;
                };
            }
        }
        
        static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

    }
}
