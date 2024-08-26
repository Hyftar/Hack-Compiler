using Compiler.Syntax;
using HyftarCSExtensions.EnumerableExtensions;

namespace Compiler.Parsers;

public interface ICombinatorialParser
{
    Task<IEnumerable<Instruction>> Parse(IEnumerable<string> lines);
}
public class CombinatorialParser : ICombinatorialParser
{
    private readonly IGenericParser[] parsers;
    private readonly ILabelBuilder labelBuilder;

    public CombinatorialParser(
        ICInstructionParser cInstructionParser,
        IAInstructionParser aInstructionParser,
        ILabelBuilder labelBuilder)
    {
        this.parsers =
            new IGenericParser[]
            {
                cInstructionParser,
                aInstructionParser
            };

        this.labelBuilder = labelBuilder;
    }

    public async Task<IEnumerable<Instruction>> Parse(IEnumerable<string> lines)
    {
        var tasks =
            this.labelBuilder
                .BuildLabelsAndRemoveLines(lines)
                .ToArray()
                .WithIndex()
                .Select((line, index) =>
                {
                    var parser = this.parsers.FirstOrDefault(p => p.CanParse(line));

                    return (parser, line, index);
                })
                .Select((parser, line, lineNumber) => parser.Parse(line, (ushort)lineNumber));

        return await Task.WhenAll(tasks);
    }
}
