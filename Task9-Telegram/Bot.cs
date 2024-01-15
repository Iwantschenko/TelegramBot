using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task9_Telegram
{
    public class Bot
    {
       
        public Bot(string path )
        {
            TelegramBotClient client = new TelegramBotClient(path);
            client.StartReceiving(Update, Error);
            
        }
      
        static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;

            if (message.Text != null)
            {
                message.Text = message.Text.ToLower();
                message.Text = message.Text.Replace(" ", string.Empty);
                if (DateTime.TryParseExact(message.Text ,"dd.MM.yyyy",null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    //do parsing
                }
                switch (message.Text)
                {
                    case "/help":
                        {
                            //do help
                        }
                        break;
                    case "/showCurrency":
                        {
                            //show currency
                        }
                        break;
                        /*
                     default:
                        {
                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "/Help", "/EnterData" }, })
                            {
                                ResizeKeyboard = true
                            };

                            InlineKeyboardMarkup inlineKeyboard = new(new[]{
                                // first row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "1.1", callbackData: "11"),
                                    InlineKeyboardButton.WithCallbackData(text: "1.2", callbackData: "12"),
                                },
                                // second row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "2.1", callbackData: "21"),
                                    InlineKeyboardButton.WithCallbackData(text: "2.2", callbackData: "22"),
                                },
                            });
                            Message seMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Оберіть дії",
                                replyMarkup: replyKeyboardMarkup,
                                cancellationToken: cancellationToken);
                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Оберіть дії",
                                replyMarkup: inlineKeyboard,
                                cancellationToken: cancellationToken);
                        }
                        break
                        */
                };
            }
        }

        static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

    }
}
