using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Utils;

namespace Day3Part1;

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

    private static string Substring(string input, int start, int length)
    {
        var effectiveStart = start > 0 ? start - 1 : 0;
        var effectiveLength = (effectiveStart + length) > input.Length ? input.Length - effectiveStart - 1 : length;
        return input.Substring(effectiveStart, effectiveLength);
    }
    
    private IEnumerable<string> GetAdjacentStrings(List<string> schematic, int lineIdx, int startPosition, int length)
    {
        // line above
        if (lineIdx > 0)
        {
            yield return Substring(schematic[lineIdx - 1], startPosition, length+2);
        }
        
        // before target
        var line = schematic[lineIdx];
        if (startPosition > 0)
        {
            yield return line.Substring(startPosition - 1, 1);
        }
        
        // following target
        if (startPosition + length < line.Length)
        {
            yield return line.Substring(startPosition + length, 1);
        }
        
        // line below
        if (lineIdx < schematic.Count - 1)
        {
            yield return Substring(schematic[lineIdx + 1], startPosition, length+2);
        }
    }

    private IEnumerable<string> GetAdjacentCharactersLists(List<string> schematic, int lineIdx, int startPosition, int length)
    {
        return GetAdjacentStrings(schematic, lineIdx, startPosition, length);
    }
    
    private IEnumerable<int> GetPartNumbers(List<string> schematic)
    {
        for (var lineIdx = 0; lineIdx < schematic.Count; lineIdx++)
        {
            var line = schematic[lineIdx];
            
            for (var i = 0; i < line.Length; i++)
            {
                if (!char.IsNumber(line[i]))
                {
                    continue;
                }
                
                var j = i + 1;
                for (; j < line.Length && char.IsNumber(line[j]); j++) {}

                var length = j - i;
                var partNumber = line.Substring(i, length);
                var adjacentCharactersList = GetAdjacentCharactersLists(schematic, lineIdx, i, length);

                if (adjacentCharactersList.Any(adjacentCharacters => adjacentCharacters.Any(c => c != '.' && !char.IsNumber(c))))
                {
                    yield return int.Parse(partNumber);
                }
                
                i = j;
            }
        }
    }
    
    public void Challenge(string[]? args)
    {
        var lines = FileReader.Lines(args).ToList();

        var partNumbers = GetPartNumbers(lines);
        Console.WriteLine(partNumbers.Sum());
    }

    [Benchmark]
    public void Benchmark()
    {
        Challenge(null);
    }
}