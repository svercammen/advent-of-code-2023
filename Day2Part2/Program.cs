using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Utils;

namespace Day2Part2;

public class Program
{
    static void Main(string[] args)
    {
        #if DEBUG
            new Program().Challenge(args.Length > 0 ? args : null);
        #else
            BenchmarkRunner.Run<Program>();
        #endif
    }

    public void Challenge(string[]? args)
    {
        var limits = new Dictionary<string, int>()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };
        
        var sum = FileReader.Lines(args).Select(line =>
            {
                // line = Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

                // split string in 2 (game X) and the list of games
                var parts = line.Split(": ");
                if (parts.Length > 2)
                {
                    throw new Exception($"Did not expect more than 2 parts in `{line}`");
                }

                // find counts
                var gameIdParts = parts[0].Split(" ");
                if (gameIdParts.Length > 2)
                {
                    throw new Exception($"Did not expect more than 2 parts in `{gameIdParts}`");
                }

                // find the highest number for each color 
                // parts[1] = 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
                var colorCounts = parts[1].Split("; ")
                    .SelectMany(set => set.Split(", ")).Select(set =>
                    {
                        var setParts = set.Split(" ");
                        return new { color = setParts[1], count = int.Parse(setParts[0]) };
                    }).Aggregate(new Dictionary<string, int>() { { "red", 0 }, { "blue", 0 }, { "green", 0 } },
                        (maxCounts, count) =>
                        {
                            maxCounts[count.color] = Math.Max(maxCounts[count.color], count.count);
                            return maxCounts;
                        });

                return colorCounts.Aggregate(1, (result, count) => result * count.Value);
            })
            .Sum();
        
        Console.WriteLine(sum);
    }

    [Benchmark]
    public void Benchmark()
    {
        Challenge(null);
    }
}