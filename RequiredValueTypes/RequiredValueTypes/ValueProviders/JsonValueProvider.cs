using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace RequiredValueTypes.RequiredValueTypes.ValueProviders;

public class JsonValueProvider : IValueProvider {
    private JsonDocument? Json { get; }

    public JsonValueProvider(string body) {
        try {
            Json = JsonDocument.Parse(body);
        } catch (JsonException) { }
    }

    public bool ContainsPrefix(string prefix) {
        return FindElement(prefix) is not null;
    }

    public ValueProviderResult GetValue(string key) {
        var result = FindElement(key);

        if (result is null) {
            return ValueProviderResult.None;
        }

        return new ValueProviderResult(new(result));
    }

    private string? FindElement(string key) {
        if (Json is null)
            return null;

        var element = Json.RootElement;

        foreach (var path in key.Split('.')) {
            var current = element;
            if (!current.TryGetProperty(path.ToLower(), out element)) {
                if (!current.TryGetProperty(path, out element)) {
                    return null;
                }
            }
        }

        return element.ToString();
    }
}
