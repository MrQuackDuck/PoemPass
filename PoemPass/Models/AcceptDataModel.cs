namespace PoemPass.Models;

public class AcceptDataModel
{
    public int Length { get; set; } = 16;
    public string Separator { get; set; } = " ";
    public bool ReverseMode { get; set; } = false;
    public bool IncludeNumbers { get; set; } = false;
    public bool RemoveAllSpecialSymbols { get; set; } = false;
    public bool CapitalizeNouns { get; set; } = false;
    public bool CapitalizePronouns { get; set; } = false;
    public bool CapitalizeVerbs { get; set; } = false;
    public bool CapitalizeAdjectives { get; set; } = false;
    public bool CapitalizeIntejections { get; set; } = false;
    public bool CapitalizePrepositions { get; set; } = false;
    public bool CapitalizeAdverbs { get; set; } = false;
}