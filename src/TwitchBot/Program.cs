using System.Threading.Tasks;
using TwitchBot.Data;

namespace TwitchBot
{
    internal static class Program
    {
        private static async Task Main()
        {
            using var context = new ApplicationDbContext();
            TwitchBot bot = new TwitchBot(context);

            while (true)
            {
                await Task.Delay(1000);
            }
        }
    }
}