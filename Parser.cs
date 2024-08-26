using Compiler.Filters;
using Compiler.Parsers;
using Compiler.Utils.FileSystem;
using HyftarCSExtensions.EnumerableExtensions;

namespace Compiler;

public interface IParser
{
    Task<ushort[]> Parse(IEnumerable<string> inputlines);

    Task Parse(string inputPath, string outputPath);
}

public class Parser : IParser
{
    private readonly IFileReader fileReader;
    private readonly IFileWriter fileWriter;
    private readonly IPrefilter prefilter;
    private readonly ICombinatorialParser combinatorialParser;

    public Parser(
        IFileReader fileReader,
        IFileWriter fileWriter,
        IPrefilter prefilter,
        ICombinatorialParser combinatorialParser)
    {
        this.fileReader = fileReader;
        this.fileWriter = fileWriter;
        this.prefilter = prefilter;
        this.combinatorialParser = combinatorialParser;
    }

    public async Task<ushort[]> Parse(IEnumerable<string> inputLines)
    {
        var filteredInput = this.prefilter.Filter(inputLines);

        var result = await this.combinatorialParser.Parse(filteredInput);

        return result.Select(x => x.ToBinary()).ToArray();
    }

    public async Task Parse(string inputPath, string outputPath)
    {
        var inputLines = this.fileReader.ReadLines(inputPath);

        var output =
            (await this.Parse(inputLines))
                .Select(x => Convert.ToString(x, 2).PadLeft(16, '0'))
                .Join(Environment.NewLine);

        this.fileWriter.WriteToFile(outputPath, output);
    }
}
