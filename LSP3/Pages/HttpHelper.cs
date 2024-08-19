using Newtonsoft.Json;

using System.Text;

namespace LSP3.Pages
{
    public class HttpHelper
    {
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T dto)
        {
            using var client = new HttpClient();
            // Serialize the DTO object to JSON
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync(url, content);

            return response;
        }

        public async Task<HttpResponseMessage> PostFactoryAsync<T>(HttpClient httpClient, string url, T dto)
        {
            // Serialize the DTO object to JSON
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            return await httpClient.PostAsync(url, content);
        }


        public async Task<string> Get(string url)
        {
            using var httpClient = new HttpClient();
            using HttpResponseMessage response = await httpClient.GetAsync(url);

			response.EnsureSuccessStatusCode(); // Check for errors

			return await response.Content.ReadAsStringAsync(); // Await again
        }

        public async Task<string> GetFactoryAsync(HttpClient httpClient, string url)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Check for errors

            return  await response.Content.ReadAsStringAsync(); // Await again
        }


    }
}
