using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Utils;

namespace Day1Part1;

[MinIterationCount(5)]
[MaxIterationCount(10)]
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Day 1 part 1");
#if DEBUG
        new Program().ChallengeLoopVariant(args.FirstOrDefault());
        new Program().ChallengeRegexVariant(args.FirstOrDefault());
#else
        BenchmarkRunner.Run<Program>();
#endif
    }
    
    private static int CalibrationValueLoopVariant(string input)
    {
        var firstDigit = "";
        var lastDigit = "";

        foreach (var character in input.Where(char.IsNumber))
        {
            if (firstDigit == "") firstDigit = character.ToString();

            lastDigit = character.ToString();
        }

        return int.Parse(firstDigit + lastDigit);
    }

    public void ChallengeLoopVariant(string? filepath)
    {
        var sum = FileReader.Lines(filepath).Sum(CalibrationValueLoopVariant);
        Console.WriteLine($"Using the loop variant: {sum}");
    }

    [Benchmark]
    public void BenchmarkLoopVariant()
    {
        ChallengeLoopVariant(null);
    }

    private readonly Regex _regex = new(@"\d", RegexOptions.Compiled);
    
    private int CalibrationValueRegexVariant(string input)
    {
        var matches = _regex.Matches(input);
        return int.Parse(matches[0].Groups[0].Value + matches[^1].Groups[0].Value);
    }

    public void ChallengeRegexVariant(string? filepath)
    {
        var sum = FileReader.Lines(filepath).Sum(CalibrationValueRegexVariant);
        Console.WriteLine($"Using the regex variant: {sum}");
    }

    [Benchmark]
    public void BenchmarkRegexVariant()
    {
        ChallengeRegexVariant(null);
    }
}
