using System.Threading.Tasks;

namespace DPFL.Bot
{
    internal static class Program
    {
        public static async Task Main(string[] args) => 
            await Startup.RunAsync(args);
    } 
}