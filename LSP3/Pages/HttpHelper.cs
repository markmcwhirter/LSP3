using LSP3.Model;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json;

namespace LSP3.Pages
{
    public class HttpHelper
    {
        public async Task<string> Get(string url)
        {
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    return  await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
