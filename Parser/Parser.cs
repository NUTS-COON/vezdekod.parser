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
        private static readonly Regex PhoneRegex = new Regex(@"[^0-9a-zа-я](?<target>\+?[\s ]?[78][\s ]?[\(\-\s][\s ]?\d{3}[\s ]?[\)\-\s][\s ]?\d{3}[\s ]?[\-\s][\s ]?\d{2}[\-\s][\s ]?\d{2})", RegexOptions.Multiline|RegexOptions.Compiled);
        private static readonly Regex IpRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex EmailRegexp = new Regex(@"(?:[a-z0-9!#$%&'*+\/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+\/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])", RegexOptions.Multiline | RegexOptions.Compiled);
        
        
        public async Task Parse(string url)
        {
            var html = await GetHtml(url);
            var phones = ParseByRegex(PhoneRegex, html);
            var ipAddresses = ParseByRegex(IpRegex, html);
            var emails = ParseByRegex(EmailRegexp, html);

            var path = "report.txt";
            var sb = new StringBuilder();
            sb.AppendLine($"Phones:\n{string.Join(",", phones)}");
            sb.AppendLine($"Ip:\n{string.Join(",", ipAddresses)}");
            sb.AppendLine($"Emails:\n{string.Join(",", emails)}");
            
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
                .Distinct()
                .ToArray();
        }
    }
}