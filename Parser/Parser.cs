using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    public class Parser
    {
        private static readonly Regex PhoneRegex = new Regex(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$",
            RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex IpRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex EmailRegexp = new Regex(@"[\\w\\.\\-]+@[\\w\\-]+\\.\\w{2,3}", RegexOptions.Multiline | RegexOptions.Compiled);
        
        
        public async Task Parse(string url)
        {
            var html = await GetHtml(url);
            var phones = ParseByRegex(PhoneRegex, html);
            var ipAddresses = ParseByRegex(IpRegex, html);
            var emails = ParseByRegex(EmailRegexp, html);

            var path = "report.txt";
            var sb = new StringBuilder();
            sb.Append($"Phones:\n{string.Join(",", phones)}");
            sb.Append($"Ip:\n{string.Join(",", ipAddresses)}");
            sb.Append($"Emails:\n{string.Join(",", emails)}");
            
            File.WriteAllText(path, sb.ToString());
        }

        public async Task<string> GetHtml(string url)
        {
            using (var c = new HttpClient())
            {
                var response = await c.GetAsync(url);

                return await response.Content.ReadAsStringAsync();
            }
        }

        private string[] ParseByRegex(Regex regex, string content)
        {
            return regex.Matches(content)
                .Select(x => x.Value)
                .ToArray();
        }
    }
}