using System.Text.Json.Serialization;

namespace PoemPass.Models;

public class Verb
{
    [JsonPropertyName("present")]
    public string Present { get; set; }
    
    [JsonPropertyName("past")]
    public string Past { get; set; }
}