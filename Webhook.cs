using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RuntimeSync
{
    public static class Webhook
    {
        public static void Send(string webhookUrl, string content)
        {
            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    username = "RuntimeSync",
                    content = content
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(webhookUrl, data).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to send webhook: {response.StatusCode}");
                }
            }
        }
    }
}