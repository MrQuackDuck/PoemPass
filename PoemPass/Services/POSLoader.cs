using System.Text.Json;
using PoemPass.Models;

namespace PoemPass.Services;

class POSLoader
{
    private readonly string nounsPath = @"./POS/nouns.json";
    private readonly string pronounsPath = @"./POS/pronouns.json";
    private readonly string adjectivesPath = @"./POS/adjectives.json";
    private readonly string adverbsPath = @"./POS/adverbs.json";
    private readonly string verbsPath = @"./POS/verbs.json";
    private readonly string prepositionsPath = @"./POS/prepositions.json";
    private readonly string interjectionsPath = @"./POS/intejections.json";
    private readonly string patternsPath = @"./POS/_patterns.json";
    private readonly string ultimatePatternsPath = @"./POS/_ultimatePatterns.json";

    public List<string> GetNouns()
    {
        string jsonString = File.ReadAllText(nounsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }

    public Dictionary<string, List<string>> GetPronouns()
    {
        string jsonString = File.ReadAllText(pronounsPath);
        var result = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString);
        return result;
    }

    public List<string> GetAdjectives()
    {
        string jsonString = File.ReadAllText(adjectivesPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }

    public List<string> GetAdverbs()
    {
        string jsonString = File.ReadAllText(adverbsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }

    public List<Verb> GetVerbs()
    {
        string jsonString = File.ReadAllText(verbsPath);
        var result = JsonSerializer.Deserialize<List<Verb>>(jsonString);
        return result;
    }
    
    public List<string> GetInterjections()
    {
        string jsonString = File.ReadAllText(interjectionsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }
    
    public List<string> GetPrepositions()
    {
        string jsonString = File.ReadAllText(prepositionsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }
    
    public List<string> GetPatterns()
    {
        string jsonString = File.ReadAllText(patternsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }

    public List<string> GetUltimatePatterns()
    {
        string jsonString = File.ReadAllText(ultimatePatternsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }
}