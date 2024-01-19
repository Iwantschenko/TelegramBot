using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System;
using BotLibrary;
using System.Configuration;
namespace Task9_Telegram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string telegramToken = ConfigurationManager.AppSettings["TelegramToken"];
            Bot bot = new Bot(telegramToken);
            string d = DateTime.Now.ToString("dd.MM.yyy");
            Console.ReadLine();
         
        }
       
        

    }
}
