using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {      
       
        /// <summary>
        /// Get Most lack endpoint
        /// </summary>
        /// <param name="format">json(default), html</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/bottleNeckProcess/", Name = "bottleNeckProcess")]
        public async Task<IActionResult> Bottleneck([FromQuery] string format = "json")
        {
            HttpClient _httpClient = new HttpClient();
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host.Value}";
//            var response = await _httpClient.GetAsync(baseUrl + "/metrics");
            var response = await _httpClient.GetAsync("https://api.optima-system.co.id/metrics");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Gagal mengambil metrics");

            var content = await response.Content.ReadAsStringAsync();
            var result = ParseMetrics(content);

            result.endPointStat = result.endPointStat
                .OrderByDescending(e => e.AverageDuration)
//                .Take(20)
                .ToList();

            if (format.ToLower() == "html")
            {
                var html = GenerateHtmlTable(result);
                return Content(html, "text/html");
            }


            return Ok(result);
        }

        private Prometheus ParseMetrics(string content)
        {
            var result = new Prometheus();
            string pattern = @"process_start_time_seconds\s+([0-9]+\.[0-9]+)";

            var match1 = Regex.Match(content, pattern);
            if (match1.Success)
            {
                double unixTimestamp = Convert.ToDouble(match1.Groups[1].Value);
                // Pisahkan detik dan pecahan
                long seconds = (long)unixTimestamp;
                double fractional = unixTimestamp - seconds;
                long milliseconds = (long)(fractional * 1000);

                // Konversi ke UTC DateTime
                var dateTimeUtc = DateTimeOffset.FromUnixTimeSeconds(seconds)
                                                 .AddMilliseconds(milliseconds)
                                                 .UtcDateTime;
                result.startTime = dateTimeUtc.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss");

            }

            var sumPattern = new Regex(@"http_request_duration_seconds_sum\{[^}]*method=""(?<method>[^""]+)"".*?endpoint=""(?<endpoint>[^""]+)"".*?\} (?<value>\d+(\.\d+)?)");
            var countPattern = new Regex(@"http_request_duration_seconds_count\{[^}]*method=""(?<method>[^""]+)"".*?endpoint=""(?<endpoint>[^""]+)"".*?\} (?<value>\d+(\.\d+)?)");
            var bucketRegex = new Regex(@"http_request_duration_seconds_bucket\{[^}]*method=""(?<method>[^""]+)"".*?endpoint=""(?<endpoint>[^""]+)"".*?le=""(?<le>[^""]+)""[^}]*\} (?<value>\d+)");

            var sumDict = new Dictionary<(string method, string endpoint), double>();
            var countDict = new Dictionary<(string method, string endpoint), double>();
            var bucketDict = new Dictionary<(string method, string endpoint), List<BucketEntry>>();

            foreach (Match match in sumPattern.Matches(content))
            {
                var method = match.Groups["method"].Value;
                var endpoint = match.Groups["endpoint"].Value;
                var value = double.Parse(match.Groups["value"].Value);
                sumDict[(method, endpoint)] = value;
            }

            foreach (Match match in countPattern.Matches(content))
            {
                var method = match.Groups["method"].Value;
                var endpoint = match.Groups["endpoint"].Value;
                var value = double.Parse(match.Groups["value"].Value);
                countDict[(method, endpoint)] = value;
            }

            foreach (Match match in bucketRegex.Matches(content))
            {
                var key = (match.Groups["method"].Value, match.Groups["endpoint"].Value);
                var le = match.Groups["le"].Value;
                var count = int.Parse(match.Groups["value"].Value);

                if (!bucketDict.ContainsKey(key))
                    bucketDict[key] = new List<BucketEntry>();

                if (double.TryParse(le, out var dur))
                {
                    bucketDict[key].Add(new BucketEntry { Duration = dur, Count = count });
                }
                else if (le == "+Inf")
                {
                    bucketDict[key].Add(new BucketEntry { Duration = double.PositiveInfinity, Count = count });
                }
            }

            result.endPointStat = new List<EndpointStat>();
            foreach (var key in sumDict.Keys)
            {
                if (countDict.TryGetValue(key, out var count) && count > 0)
                {
                    var avg = sumDict[key] / count;
                    result.endPointStat.Add(new EndpointStat
                    {
                        Method = key.method,
                        Endpoint = key.endpoint,
                        TotalDuration = sumDict[key],
                        AverageDuration = avg,
                        RequestCount = count,
                        Buckets = bucketDict.ContainsKey(key) ? bucketDict[key] : new List<BucketEntry>()
                    });
                }
            }

            return result;
        }
        private string GenerateHtmlTable(Prometheus stats)
        {
            var sb = new StringBuilder();
            sb.Append("<html><head><style>");
            sb.Append("body { font-family: sans-serif; margin: 2rem; }");
            sb.Append("table { border-collapse: collapse; width: 100%; margin-bottom: 1rem; }");
            sb.Append("th, td { border: 1px solid #ccc; padding: 8px; text-align: left; }");
            sb.Append("th { background-color: #f4f4f4; }");
            sb.Append(".details { display: none; margin-top: 0.5rem; }");
            sb.Append(".row:hover { background-color: #eef; cursor: pointer; }");
            sb.Append(".info {text-align: left;margin-top: 20px;font-size: 12px;color: #888;}");
            sb.Append("</style>");
            sb.Append("<script>");
            sb.Append(@"
        function toggleDetails(id) {
            var el = document.getElementById(id);
            if (el.style.display === 'none') {
                el.style.display = 'block';
            } else {
                el.style.display = 'none';
            }
        }");
            sb.Append("</script></head><body>");
            sb.Append("<h2>Endpoints Load AVG</h2>");
            sb.Append($"<div class=\"info\"><p>Start Time:{stats.startTime} </div>");
            sb.Append("<table><tr><th>Method</th><th>Endpoint</th><th>Total Duration (s)</th><th>Avg Duration (s)</th><th>Requests</th></tr>");

            int i = 0;
            foreach (var item in stats.endPointStat)
            {
                string id = $"details_{i}";
                sb.Append($"<tr class='row' title='Klik untuk lihat distribusi request' onclick=\"toggleDetails('{id}')\">");
                sb.Append($"<td>{item.Method}</td>");
                sb.Append($"<td>{item.Endpoint}</td>");
                sb.Append($"<td>{item.TotalDuration:F4}</td>");
                sb.Append($"<td>{item.AverageDuration:F4}</td>");
                sb.Append($"<td>{item.RequestCount}</td>");
                sb.Append("</tr>");

                if (item.Buckets != null && item.Buckets.Any())
                {
                    sb.Append($"<tr><td colspan='5'><div id='{id}' class='details'>");
                    sb.Append("<b>Distribusi Durasi (Bucket):</b>");
                    sb.Append($"<table><tr><th>&le; Durasi (s)</th><th>Request Count</th></tr>");
                    foreach (var bucket in item.Buckets)
                    {
                       string durText = double.IsInfinity(bucket.Duration) ? "&infin;" : bucket.Duration.ToString("F4");

                        sb.Append($"<tr><td>{durText}</td><td>{bucket.Count}</td></tr>");
                    }
                    sb.Append("</table></div></td></tr>");
                }

                i++;
            }

            sb.Append("</table></body></html>");
            return sb.ToString();
        }
        public class Prometheus
        {
            public string startTime { get; set; }
            public List<EndpointStat> endPointStat { get; set; }
        }
        public class EndpointStat
        {
            public string Method { get; set; } = string.Empty;
            public string Endpoint { get; set; } = string.Empty;
            public double TotalDuration { get; set; }
            public double AverageDuration { get; set; }
            public double RequestCount { get; set; }
            public List<BucketEntry> Buckets { get; set; } = new();
        }

        public class BucketEntry
        {
            public double Duration { get; set; }
            public int Count { get; set; }
        }
    }
}
