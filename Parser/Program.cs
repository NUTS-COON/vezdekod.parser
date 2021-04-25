using System;
using System.Threading.Tasks;

namespace Parser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine($"Usage: {args[0]} url");
            }
            var url = args[0];
            var parser = new Parser();

            await parser.Parse(url);
        }
    }
}