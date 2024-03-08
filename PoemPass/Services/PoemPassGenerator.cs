using System.Text;
using System.Text.RegularExpressions;
using PoemPass.Models;

namespace PoemPass.Services;

public class PoemPassGenerator
{
    public PoemPassGenerator()
    {
        _posLoader = new PosLoader();
    }
    
    private PosLoader _posLoader;
    
    public string ConvertPoemToPassword(string poem, bool reverseMode, string separator = " ")
    {
        StringBuilder passwordBuilder = new StringBuilder();
        foreach (var word in GetWords(poem, separator))
        {
            if (word.Length < 1) continue;
            
            // Handling reverse mode
            if (reverseMode is true && word.Length > 1)
            {
                if (Char.IsUpper(word[0])) passwordBuilder.Append(Char.ToUpper(word[word.Length - 1]));
                else passwordBuilder.Append(word[word.Length - 1]);
            }
            else passwordBuilder.Append(word[0]);
        }

        return passwordBuilder.ToString();
    }
    
    public UserResponse GeneratePoemPass(AcceptDataModel options = null)
    {
        if (options is null) options = new AcceptDataModel();

        var patterns = _posLoader.GetSentencePatterns();
        
        string targetPattern;
        
        // Getting random patter
        do targetPattern = patterns[GetRandomValue(0, patterns.Count)];
        while (targetPattern.Contains("[NUMERAL]") && !options.IncludeNumbers);
        
        // Update targetPattern until it contains numeral part of speech in it
        while (!targetPattern.Contains("[NUMERAL]") && options.IncludeNumbers)
            targetPattern = patterns[GetRandomValue(0, patterns.Count)];

        // Replace parts of speech
        targetPattern = ProcessPattern(targetPattern, options);

        if (GetRandomValue(1, 3) == 2) targetPattern = UpperFirstSymbol(targetPattern);
        else targetPattern = LowerFirstSymbol(targetPattern);
        
        while (GetWordsCount(targetPattern) != options.Length)
        {
            var sentenceContinuationPatterns = _posLoader.GetSentenceContinuationPatterns();
            string randomSentenceContinuation = sentenceContinuationPatterns[GetRandomValue(0, sentenceContinuationPatterns.Count)];
            randomSentenceContinuation = ProcessPattern(randomSentenceContinuation, options);
            targetPattern += randomSentenceContinuation;

            while (GetWordsCount(targetPattern) > options.Length)
                targetPattern = RemoveLastWord(targetPattern);

            if (options.RemoveAllSpecialSymbols) targetPattern = RemoveAllSpecialSymbols(targetPattern);
        }

        targetPattern = targetPattern.Replace(" ", options.Separator);

        UserResponse response = new UserResponse()
        {
            Password = ConvertPoemToPassword(targetPattern, options.ReverseMode, options.Separator),
            Poem = targetPattern,
        };
        
        return response;
    }
    
    private int GetRandomValue(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
    
    private string ReplaceFirst(string text, string search, string replace)
    {
        int posistion = text.IndexOf(search);
        if (posistion < 0) return text;
        
        return text.Substring(0, posistion) + replace + text.Substring(posistion + search.Length);
    }

    private static string RemoveLastWord(string text)
    {
        int lastSpaceIndex = text.LastIndexOf(' ');
        string newSentence = text.Substring(0, lastSpaceIndex);
        return newSentence;
    }

    private string UpperFirstSymbol(string text)
    {
        if (text.Length > 1) return text.First().ToString().ToUpper() + text.Substring(1);
        return text;
    }

    private string LowerFirstSymbol(string text)
    {
        if (text.Length > 1) return text.First().ToString().ToLower() + text.Substring(1);
        else return text;
    }

    private string ReplaceNouns(string pattern, bool uppercase = false)
    {
        if (!pattern.Contains("[NOUN]")) return pattern;
        
        int nounsCountInPattern = Regex.Matches(pattern, @"\[NOUN\]").Count;
        var nouns = _posLoader.GetNouns();
        for (int i = 0; i < nounsCountInPattern; i++)
        {
            string randomNoun = nouns[GetRandomValue(0, nouns.Count)];
            if (uppercase is true) randomNoun = UpperFirstSymbol(randomNoun);
            pattern = ReplaceFirst(pattern, "[NOUN]", randomNoun);
        }

        return pattern;
    }

    private string ReplacePronouns(string pattern, bool uppercase = false)
    {
        foreach (var keyValuePair in _posLoader.GetPronouns())
        {
            if (!pattern.Contains(keyValuePair.Key)) continue;
            
            int pronounsCountInPattern = Regex.Matches(pattern, keyValuePair.Key).Count;
            var pronouns = keyValuePair;
            for (int i = 0; i < pronounsCountInPattern; i++)
            {
                string randomPronoun = pronouns.Value[GetRandomValue(0, pronouns.Value.Count)];
                if (uppercase is true) randomPronoun = UpperFirstSymbol(randomPronoun);
                pattern = ReplaceFirst(pattern, keyValuePair.Key, randomPronoun);
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
                if (uppercase) randomVerb = UpperFirstSymbol(randomVerb);
                pattern = ReplaceFirst(pattern, "[VERB_PAST]", randomVerb);
            }
        }

        return pattern;
    }

    private string ReplaceAdjectives(string pattern, bool uppercase = false)
    {
        if (!pattern.Contains("[ADJECTIVE]")) return pattern;
        
        int adjectivesCount = Regex.Matches(pattern, @"\[ADJECTIVE\]").Count;
        var adjectives = _posLoader.GetAdjectives();
        for (int i = 0; i < adjectivesCount; i++)
        {
            string randomAdjective = adjectives[GetRandomValue(0, adjectives.Count)];
            if (uppercase is true) randomAdjective = UpperFirstSymbol(randomAdjective);
            pattern = ReplaceFirst(pattern, "[ADJECTIVE]", randomAdjective);
        }

        return pattern;
    }

    private string ReplaceIntejections(string pattern, bool uppercase = false)
    {
        if (!pattern.Contains("[INTERJECTION]")) return pattern;
        
        int interjectionsCount = Regex.Matches(pattern, @"\[INTERJECTION\]").Count;
        var interjections = _posLoader.GetInterjections();
        for (int i = 0; i < interjectionsCount; i++)
        {
            string randomInterjection = interjections[GetRandomValue(0, interjections.Count)];
            if (uppercase is true) randomInterjection = UpperFirstSymbol(randomInterjection);
            pattern = ReplaceFirst(pattern, "[INTERJECTION]", randomInterjection);
        }

        return pattern;   
    }

    private string ReplaceNumerals(string pattern)
    {
        if (!pattern.Contains("[NUMERAL]")) return pattern;
        
        int numeralsCount = Regex.Matches(pattern, @"\[NUMERAL\]").Count;
        for (int i = 0; i < numeralsCount; i++)
        {
            string randomNumeral = GetRandomValue(0, 10).ToString();
            pattern = ReplaceFirst(pattern, "[NUMERAL]", randomNumeral);
        }
    

        return pattern;
    }
    
    private string ReplacePrepositions(string pattern, bool uppercase = false)
    {
        if (!pattern.Contains("[PREPOSITION]")) return pattern;

        int prepositionCount = Regex.Matches(pattern, @"\[PREPOSITION\]").Count;
        var prepositions = _posLoader.GetPrepositions();
        for (int i = 0; i < prepositionCount; i++)
        {
            string randomPreposition = prepositions[GetRandomValue(0, prepositions.Count)];
            if (uppercase is true) randomPreposition = UpperFirstSymbol(randomPreposition);
            pattern = ReplaceFirst(pattern, "[PREPOSITION]", randomPreposition);
        }

        return pattern;   
    }

    private string ReplaceAdverbs(string pattern, bool uppercase = false)
    {
        if (!pattern.Contains("[ADVERB]")) return pattern;
        
        int adverbCount = Regex.Matches(pattern, @"\[ADVERB\]").Count;
        var adverbs = _posLoader.GetAdverbs();
        for (int i = 0; i < adverbCount; i++)
        {
            string randomAdverb = adverbs[GetRandomValue(0, adverbs.Count)];
            if (uppercase is true) UpperFirstSymbol(randomAdverb);
            pattern = ReplaceFirst(pattern, "[ADVERB]", randomAdverb);
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
    
    private string[] GetWords(string text) => text.Split(' ');
    
    private string[] GetWords(string text, string separator) => text.Split(separator);

    private int GetWordsCount(string text) => GetWords(text).Length;

    private string RemoveAllSpecialSymbols(string text) 
        => Regex.Replace(text, "[*'\",_&#^@!?]", "");
}