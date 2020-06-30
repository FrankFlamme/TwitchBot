using System.Threading.Tasks;
using TwitchBot.Data;

namespace TwitchBot
{
    public static class Program
    {
        public static async Task Main()
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