using System.Text.Json;
using PoemPass.Models;

namespace PoemPass.Services;

/// <summary>
/// Class to load Parts of Speech (like nouns, adjectives etc.)
/// </summary>
class PosLoader
{
    private readonly string nounsPath = @"./PartsOfSpeech/nouns.json";
    private readonly string pronounsPath = @"./PartsOfSpeech/pronouns.json";
    private readonly string adjectivesPath = @"./PartsOfSpeech/adjectives.json";
    private readonly string adverbsPath = @"./PartsOfSpeech/adverbs.json";
    private readonly string verbsPath = @"./PartsOfSpeech/verbs.json";
    private readonly string prepositionsPath = @"./PartsOfSpeech/prepositions.json";
    private readonly string interjectionsPath = @"./PartsOfSpeech/intejections.json";
    private readonly string patternsPath = @"./PartsOfSpeech/_patterns.json";
    private readonly string sentenceContinuationPatternsPath = @"./PartsOfSpeech/_sentenceContinuationPatterns.json";

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
    
    public List<string> GetSentencePatterns()
    {
        string jsonString = File.ReadAllText(patternsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }

    public List<string> GetSentenceContinuationPatterns()
    {
        string jsonString = File.ReadAllText(sentenceContinuationPatternsPath);
        var result = JsonSerializer.Deserialize<List<string>>(jsonString);
        return result;
    }
}