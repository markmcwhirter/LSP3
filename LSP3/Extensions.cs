using System.Text.Json;

namespace LSP3
{
    public class Extensions<T> where T : new()
    {
        public T Deserialize(string serializedString) 
        {
            if (serializedString == null)
            {
                return new T();
            }
            return JsonSerializer.Deserialize<T>(serializedString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }

}
