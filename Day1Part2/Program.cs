using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Utils;

namespace Day1Part2;

public partial class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Day 1 part 2");

        #if DEBUG
            new Program().Challenge(args.Length > 0 ? args : null);
        #else
            BenchmarkRunner.Run<Program>();
        #endif
    }

    private readonly Dictionary<string, int> _mapping = new()
    {
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9},
    }; 
    private int ConvertValue(string input)
    {
        return _mapping.GetValueOrDefault(input);
    }
    
    private const string Pattern = @"(one|two|three|four|five|six|seven|eight|nine|\d)";
    private readonly Regex _regex = GetRegex();
    private readonly Regex _regexReverse = GetReversedRegex();
    private int CalibrationValue(string input)
    {
        var firstMatch = _regex.Match(input);
        var lastMatch = _regexReverse.Match(input);
        
        return ConvertValue(firstMatch.Value) * 10 + ConvertValue(lastMatch.Value);
    }
    
    public void Challenge(string[]? args)
    {
        var sum = FileReader.Lines(args).Sum(CalibrationValue);
        Console.WriteLine(sum);
    }

    [Benchmark]
    public void Benchmark()
    {
        Challenge(null);
    }

    [GeneratedRegex(Pattern)]
    private static partial Regex GetRegex();
    [GeneratedRegex(Pattern, RegexOptions.RightToLeft)]
    private static partial Regex GetReversedRegex();
}