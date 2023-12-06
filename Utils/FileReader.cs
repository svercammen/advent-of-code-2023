namespace Utils;

public class FileReader
{
    public static IEnumerable<string> Lines(string? path)
    {
        string[] paths = path != null
            ? [path]
            : [
                "input.txt",    // dotnet watch
                "../../../../../../../input.txt",   // benchmark runner
                "../../input.txt"   // debugger
            ];
        
        var file = paths.FirstOrDefault(File.Exists);
        if (file == null)
        {
            throw new Exception($"Unable to find viable file in [{string.Join(", ", paths)}]");
        }
        
        var sr = new StreamReader(file);
        var line = sr.ReadLine();
        while (line != null)
        {
            yield return line;

            line = sr.ReadLine();
        }
    }
}