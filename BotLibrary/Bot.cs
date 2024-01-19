using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
namespace BotLibrary
{
    public class Bot
    {
        private bool IsBoardCreate = false;
        private ResourceManager _resource;
        private CachingCurrency _currencyCache;

        public Bot(string path)
        {
            _resource = new ResourceManager("BotLibrary.Resource", Assembly.GetExecutingAssembly());
            var cache = new MemoryCache(new MemoryCacheOptions());
            _currencyCache = new CachingCurrency(cache);
            TelegramBotClient client = new TelegramBotClient(path);
            client.StartReceiving(Update, Error);


        }
       
        static public bool InputDateCheck(DateTime date)
        {
            if (date <= DateTime.Now && date.Year >= DateTime.Now.Year - 4)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                                text: _resource.GetString("CreateBoard"),
                                replyMarkup: replyKeyboardMarkup,
                                cancellationToken: cancellationToken);
                IsBoardCreate = true;
            }
            if (message.Text != null)
            {
                message.Text = message.Text.ToLower();
                message.Text = message.Text.Replace(" ", string.Empty);

                if (message.Text.Length == 3)
                {
                    Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: _currencyCache.GetCurrency(message.Text),
                                cancellationToken: cancellationToken);
                    return;
                }

                if (DateTime.TryParse(message.Text , out DateTime date))
                {

                    if (InputDateCheck(date))
                    {
                        _currencyCache.RefreshCurrency(date);
                        Message AccesMessage = await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: $"{_resource.GetString("ChangeDate")} {{{date}}})",
                                    cancellationToken: cancellationToken);
                    }
                    else
                    {
                        Message errorMessage = await botClient.SendTextMessageAsync(
                                    chatId: message.Chat.Id,
                                    text: _resource.GetString("ErrorDate"),
                                    cancellationToken: cancellationToken);
                    }
                    return;
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
                            Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: _resource.GetString("ChooseDateKey"),
                                cancellationToken: cancellationToken);
                        }
                        break;
                    default:
                        {
                            Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: $"{_resource.GetString("DefaultAnswer")} ({message.Text})",
                                cancellationToken: cancellationToken);
                        }
                        break;
                };
            }
            else
            {
                Message seMessage = await botClient.SendTextMessageAsync(
                               chatId: message.Chat.Id,
                               text: _resource.GetString("TextNotNullReference"),
                               cancellationToken: cancellationToken);
            }
        }

        public Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

    }
}
