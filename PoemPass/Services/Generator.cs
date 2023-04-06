using System.Text;
using System.Text.RegularExpressions;
using PoemPass.Enums;
using PoemPass.Models;

namespace PoemPass.Services;

public class Generator
{
    public Generator()
    {
        _posLoader = new POSLoader();
    }
    
    private POSLoader _posLoader;
    
    private int GetRandomValue(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
    
    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }

    public static string RemoveLastWord(string text)
    {
        int lastSpaceIndex = text.LastIndexOf(' ');
        string newSentence = text.Substring(0, lastSpaceIndex);
        return newSentence;
    }

    private string UpperFirstSymbol(string text)
    {
        if (text.Length > 1)
            return text.First().ToString().ToUpper() + text.Substring(1);
        return text;
    }

    private string LowerFirstSymbol(string text)
    {
        if (text.Length > 1)
            return text.First().ToString().ToLower() + text.Substring(1);
        else
            return text;
    }

    private string ReplaceNouns(string pattern, bool uppercase = false)
    {
        if (pattern.Contains("[NOUN]"))
        {
            int nounsCountInPattern = Regex.Matches(pattern, @"\[NOUN\]").Count;
            var nouns = _posLoader.GetNouns();
            for (int i = 0; i < nounsCountInPattern; i++)
            {
                string randomNoun = nouns[GetRandomValue(0, nouns.Count)];
                if (uppercase is true) randomNoun = UpperFirstSymbol(randomNoun);
                pattern = ReplaceFirst(pattern, "[NOUN]", randomNoun);
            }
        }

        return pattern;
    }

    private string ReplacePronouns(string pattern, bool uppercase = false)
    {
        foreach (var keyValuePair in _posLoader.GetPronouns())
        {
            if (pattern.Contains(keyValuePair.Key))
            {
                int pronounsCountInPattern = Regex.Matches(pattern, keyValuePair.Key).Count;
                var pronouns = keyValuePair;
                for (int i = 0; i < pronounsCountInPattern; i++)
                {
                    string randomPronoun = pronouns.Value[GetRandomValue(0, pronouns.Value.Count)];
                    if (uppercase is true) randomPronoun = UpperFirstSymbol(randomPronoun);
                    pattern = ReplaceFirst(pattern, keyValuePair.Key, randomPronoun);
                }
            }
        }

        return pattern;
    }

    private string ReplaceVerbs(string pattern, bool uppercase = false)
    {
        if (pattern.Contains("[VERB]"))
        {
            int verbsCount = Regex.Matches(pattern, @"\[VERB\]").Count;
            var verbs = _posLoader.GetVerbs();
            for (int i = 0; i < verbsCount; i++)
            {
                string randomVerb = verbs[GetRandomValue(0, verbs.Count)].Present;
                if (uppercase is true) randomVerb = UpperFirstSymbol(randomVerb);
                pattern = ReplaceFirst(pattern, "[VERB]", randomVerb);
            }
        }        
        if (pattern.Contains("[VERB_PAST]"))
        {
            int verbsCount = Regex.Matches(pattern, @"\[VERB_PAST\]").Count;
            var verbs = _posLoader.GetVerbs();
            for (int i = 0; i < verbsCount; i++)
            {
                string randomVerb = verbs[GetRandomValue(0, verbs.Count)].Past;
                if (uppercase is true) randomVerb = UpperFirstSymbol(randomVerb);
                pattern = ReplaceFirst(pattern, "[VERB_PAST]", randomVerb);
            }
        }

        return pattern;
    }

    private string ReplaceAdjectives(string pattern, bool uppercase = false)
    {
        if (pattern.Contains("[ADJECTIVE]"))
        {
            int adjectivesCount = Regex.Matches(pattern, @"\[ADJECTIVE\]").Count;
            var adjectives = _posLoader.GetAdjectives();
            for (int i = 0; i < adjectivesCount; i++)
            {
                string randomAdjective = adjectives[GetRandomValue(0, adjectives.Count)];
                if (uppercase is true) randomAdjective = UpperFirstSymbol(randomAdjective);
                pattern = ReplaceFirst(pattern, "[ADJECTIVE]", randomAdjective);
            }
        }

        return pattern;
    }

    private string ReplaceIntejections(string pattern, bool uppercase = false)
    {
        if (pattern.Contains("[INTERJECTION]"))
        {
            int interjectionsCount = Regex.Matches(pattern, @"\[INTERJECTION\]").Count;
            var interjections = _posLoader.GetInterjections();
            for (int i = 0; i < interjectionsCount; i++)
            {
                string randomInterjection = interjections[GetRandomValue(0, interjections.Count)];
                if (uppercase is true) randomInterjection = UpperFirstSymbol(randomInterjection);
                pattern = ReplaceFirst(pattern, "[INTERJECTION]", randomInterjection);
            }
        }

        return pattern;   
    }

    private string ReplaceNumerals(string pattern)
    {
        if (pattern.Contains("[NUMERAL]"))
        {
            int numeralsCount = Regex.Matches(pattern, @"\[NUMERAL\]").Count;
            for (int i = 0; i < numeralsCount; i++)
            {
                string randomNumeral = GetRandomValue(0, 10).ToString();
                pattern = ReplaceFirst(pattern, "[NUMERAL]", randomNumeral);
            }
        }

        return pattern;
    }
    
    private string ReplacePrepositions(string pattern, bool uppercase = false)
    {
        if (pattern.Contains("[PREPOSITION]"))
        {
            int prepositionCount = Regex.Matches(pattern, @"\[PREPOSITION\]").Count;
            var prepositions = _posLoader.GetPrepositions();
            for (int i = 0; i < prepositionCount; i++)
            {
                string randomPreposition = prepositions[GetRandomValue(0, prepositions.Count)];
                if (uppercase is true) randomPreposition = UpperFirstSymbol(randomPreposition);
                pattern = ReplaceFirst(pattern, "[PREPOSITION]", randomPreposition);
            }
        }

        return pattern;   
    }

    private string ReplaceAdverbs(string pattern, bool uppercase = false)
    {
        if (pattern.Contains("[ADVERB]"))
        {
            int adverbCount = Regex.Matches(pattern, @"\[ADVERB\]").Count;
            var adverbs = _posLoader.GetAdverbs();
            for (int i = 0; i < adverbCount; i++)
            {
                string randomAdverb = adverbs[GetRandomValue(0, adverbs.Count)];
                if (uppercase is true) UpperFirstSymbol(randomAdverb);
                pattern = ReplaceFirst(pattern, "[ADVERB]", randomAdverb);
            }
        }

        return pattern;   
    }

    private string ProcessPattern(string pattern, AcceptDataModel options)
    {
        pattern = ReplaceNouns(pattern, options.CapitalizeNouns);
        pattern = ReplacePronouns(pattern, options.CapitalizePronouns);
        pattern = ReplaceVerbs(pattern, options.CapitalizeVerbs);
        pattern = ReplaceAdjectives(pattern, options.CapitalizeAdjectives);
        pattern = ReplaceIntejections(pattern, options.CapitalizeIntejections);
        pattern = ReplacePrepositions(pattern, options.CapitalizePrepositions);
        pattern = ReplaceAdverbs(pattern, options.CapitalizeAdverbs);
        pattern = ReplaceNumerals(pattern);
        return pattern;
    }
    
    private string[] GetWords(string text)
    {
        return text.Split(' ');
    }
    
    private string[] GetWords(string text, string separator)
    {
        return text.Split(separator);
    }

    private int GetWordsCount(string text)
    {
        return GetWords(text).Length;
    }

    public string ConvertPoemToPassword(string poem, bool reverseMode, string separator = " ")
    {
        StringBuilder result = new StringBuilder();
        foreach (var word in GetWords(poem, separator))
        {
            if (word.Length < 1) continue;
            if (reverseMode is true && word.Length > 1)
            {
                if (Char.IsUpper(word[0]))
                    result.Append(Char.ToUpper(word[word.Length - 1]));
                else
                    result.Append(word[word.Length - 1]);
            }
            else
            {
                result.Append(word[0]);   
            }
        }

        return result.ToString();
    }

    private string RemoveAllSpecialSymbols(string text)
    {
        return Regex.Replace(text, "[*'\",_&#^@!?]", "");
    }
    
    public Response Generate(AcceptDataModel options = null)
    {
        if (options is null) options = new AcceptDataModel();

        var patterns = _posLoader.GetPatterns();
        
        // Get random pattern
        string targetPattern = "";
        do
        {
            targetPattern = patterns[GetRandomValue(0, patterns.Count)];
        } while (targetPattern.Contains("[NUMERAL]") && options.IncludeNumbers is false);
        
        while (!targetPattern.Contains("[NUMERAL]") && options.IncludeNumbers is true)
        {
            targetPattern = patterns[GetRandomValue(0, patterns.Count)];
        }

        // Replace parts of speech
        targetPattern = ProcessPattern(targetPattern, options);

        if (GetRandomValue(1, 3) == 2) targetPattern = UpperFirstSymbol(targetPattern);
        else targetPattern = LowerFirstSymbol(targetPattern);
        
        while (GetWordsCount(targetPattern) != options.Length)
        {
            var ultimatePatterns = _posLoader.GetUltimatePatterns();
            string randomUltimatePattern = ultimatePatterns[GetRandomValue(0, ultimatePatterns.Count)];
            randomUltimatePattern = ProcessPattern(randomUltimatePattern, options);
            targetPattern = targetPattern + randomUltimatePattern;

            while (GetWordsCount(targetPattern) > options.Length)
            {
                targetPattern = RemoveLastWord(targetPattern);
            }
            if (options.RemoveAllSpecialSymbols is true) targetPattern = RemoveAllSpecialSymbols(targetPattern);
        }

        targetPattern = targetPattern.Replace(" ", options.Separator);

        Response response = new Response()
        {
            Password = ConvertPoemToPassword(targetPattern, options.ReverseMode, options.Separator),
            Poem = targetPattern,
        };
        
        return response;
    }
}