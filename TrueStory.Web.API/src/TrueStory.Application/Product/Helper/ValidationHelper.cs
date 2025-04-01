using System.Text.RegularExpressions;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Dynamic;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace TrueStory.Application.Product.Helper;

public static class ValidationHelper
{
    /// <summary>
    /// Validates that the input string contains only English letters (if it contains any letters).
    /// Allows numbers, hyphens (-), parentheses (), and other special characters.
    /// Examples of valid names: "Product-123", "My-Product(2024)", "Product_Name", "123Product"
    /// </summary>
    /// <param name="name">The string to validate.</param>
    /// <returns>true if the string contains only English letters (if it contains any letters); otherwise, false.</returns>
    public static bool ValidateForMixedLanguages(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;

        return !name.Any(c => char.IsLetter(c) && !IsEnglishLetter(c));
    }

    /// <summary>
    /// Validates if the input object is in valid JSON format.
    /// </summary>
    /// <param name="data">The JsonElement to validate.</param>
    /// <returns>true if the object is valid JSON; otherwise, false.</returns>
    public static bool ValidateJsonFormat(JsonElement data)
    {
        try
        {
            //Convert to JSON string and back to ensure it's valid JSON
            var jsonString = JsonConvert.SerializeObject(data);
            JsonDocument.Parse(jsonString);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsEnglishLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
}
