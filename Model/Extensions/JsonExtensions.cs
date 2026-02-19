using System.Text.Json;

namespace Model.Extensions;

public static class JsonExtensions
{
    public static string ToJson<T>(this T obj) where T : class
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
