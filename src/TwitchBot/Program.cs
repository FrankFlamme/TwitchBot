using System;
using System.Threading.Tasks;
using TwitchBot.Data;

namespace TwitchBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new ApplicationDbContext();
            TwitchBot bot = new TwitchBot(context);

            while(true)
            {
                await Task.Delay(1000);
            }
        }
    }
}
