using Compiler.Syntax;

namespace Compiler.Parsers;

public interface IGenericParser 
{
    Task<Instruction> Parse(string line, ushort lineNumber);

    bool CanParse(string line);
}
