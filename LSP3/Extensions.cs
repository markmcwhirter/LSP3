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
#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<T>(serializedString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
#pragma warning restore CS8603 // Possible null reference return.
        }
    }

}
