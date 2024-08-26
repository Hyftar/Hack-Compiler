using Compiler.Syntax;
using HyftarCSExtensions.EnumerableExtensions;
using System.Text.RegularExpressions;

namespace Compiler.Parsers;

public interface ILabelBuilder
{
    bool IsLabelLine(string line);

    IEnumerable<string> BuildLabelsAndRemoveLines(IEnumerable<string> lines);
}

public class LabelBuilder : ILabelBuilder
{
    private readonly ISymbolsTable symbolsTable;

    private static readonly Regex LABEL_PATTERN = new(@"^\((?<label>[^\s\d]\S*)\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private ushort lineOffset = 0;

    public LabelBuilder(ISymbolsTable symbolsTable)
    {
        this.symbolsTable = symbolsTable;
    }

    public bool IsLabelLine(string line) => LABEL_PATTERN.IsMatch(line);

    public IEnumerable<string> BuildLabelsAndRemoveLines(IEnumerable<string> lines)
    {
        foreach (var (line, lineNumber) in lines.WithIndex())
        {
            var match = LABEL_PATTERN.Match(line);

            if (match.Success)
            {
                this.symbolsTable.AddLabel(match.Groups["label"].Value, (ushort)(lineNumber - this.lineOffset));
                this.lineOffset += 1;

                continue;
            }

            yield return line;
        }
    }
}
