using Compiler.Syntax;
using System.Text.RegularExpressions;

namespace Compiler.Parsers;

public interface IAInstructionParser : IGenericParser { }

public class AInstructionParser : IAInstructionParser
{
    private static readonly Regex SYMBOL_PATTERN = new(@"^@(?<symbol>[^\s\d]\S*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private readonly ISymbolsTable symbolsTable;

    public AInstructionParser(ISymbolsTable symbolsTable)
    {
        this.symbolsTable = symbolsTable;
    }

    public bool CanParse(string line) => line.StartsWith('@');

    public Task<Instruction> Parse(string line, ushort lineNumber)
    {
        var match = SYMBOL_PATTERN.Match(line);

        if (match.Success)
        {
            return Task.FromResult<Instruction>(
                new AInstruction
                {
                    LineNumber = lineNumber,
                    RawInstruction = line,
                    Value = this.symbolsTable.GetReference(match.Groups["symbol"].Value)
                }
            );
        }
        
        var value = ushort.Parse(line[1..]);

        return Task.FromResult<Instruction>(
            new AInstruction
            {
                LineNumber = lineNumber,
                RawInstruction = line,
                Value = value
            }
        );
    }
}
