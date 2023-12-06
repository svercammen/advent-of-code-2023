using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

Console.WriteLine("Day 1");

#if DEBUG
    Day1.ChallengeLoopVariant("input.txt");
    new Day1().ChallengeRegexVariant("input.txt");
#else
    BenchmarkRunner.Run<Day1>();
#endif

[MinIterationCount(5)]
[MaxIterationCount(10)]
public class Day1
{
    private static IEnumerable<string> Lines(string? path)
    {
        var sr = new StreamReader(path ?? DefaultPath());

        var line = sr.ReadLine();
        while (line != null)
        {
            yield return line;

            line = sr.ReadLine();
        }
    }

    private static string DefaultPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "../../../../../../..", "input.txt");
    }

    private static int CalibrationValueLoopVariant(string input)
    {
        var firstDigit = "";
        var lastDigit = "";

        foreach (var character in input.Where(char.IsNumber))
        {
            if (firstDigit == "")
            {
                firstDigit = character.ToString();
            }
                
            lastDigit = character.ToString();
        }

        return int.Parse(firstDigit + lastDigit);
    }
    
    public static void ChallengeLoopVariant(String? filepath)
    {
        var sum = Lines(filepath).Sum(CalibrationValueLoopVariant);
        Console.WriteLine($"Using the loop variant: {sum}");
    }

    [Benchmark]
    public void BenchmarkChallengeLoopVariant()
    {
        ChallengeLoopVariant(null);
    }

    private readonly Regex _regex = new Regex(@"\d", RegexOptions.Compiled);

    private int CalibrationValueRegexVariant(string input)
    {
        var matches = _regex.Matches(input);
        return int.Parse(matches[0].Groups[0].Value + matches[^1].Groups[0].Value);
    }
    
    public void ChallengeRegexVariant(string? filepath)
    {
        var sum = Lines(filepath).Sum(CalibrationValueRegexVariant);
        Console.WriteLine($"Using the regex variant: {sum}");
    }
    
    [Benchmark]
    public void BenchmarkChallengeRegexVariant()
    {
        this.ChallengeRegexVariant(null);
    }
}