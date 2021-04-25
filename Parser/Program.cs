using System;
using System.Threading.Tasks;

namespace Parser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = args[0];
            var parser = new Parser();

            await parser.Parse(url);
        }
    }
}