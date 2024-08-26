using Castle.Core.Internal;
using Compiler.Syntax;
using System.Text.RegularExpressions;

namespace Compiler.Parsers;

public interface ICInstructionParser : IGenericParser { }

public class CInstructionParser : ICInstructionParser
{
    private static readonly Regex C_INST_PATTERN =
        new(
            @"
                ^
                    (
                        (?<dest>[AMD]{1,3})
                        =
                    )?
                    (?<comp>
                        -?[01]|
                        [-!]?[DA]|
                        D[+-][1AM]|
                        D[&|][AM]|
                        A[+-][1D]|
                        M[+-][1D]|
                        [-!]?M
                    )
                    (
                        ;
                        (?<jump>
                            JGT|
                            JEQ|
                            JGE|
                            JLT|
                            JMP|
                            JLE|
                            JNE
                        )
                    )?
                $
            ",
            RegexOptions.IgnoreCase |
            RegexOptions.Compiled |
            RegexOptions.IgnorePatternWhitespace |
            RegexOptions.ExplicitCapture
        );

    public bool CanParse(string line) => C_INST_PATTERN.IsMatch(line);

    public Task<Instruction> Parse(string line, ushort lineNumber)
    {
        var match = C_INST_PATTERN.Match(line);

        var (computationBits, isAlternativeComp) = GetComputationBits(match.Groups["comp"].Value);

        return
            Task.FromResult<Instruction>(
                new CInstruction()
                {
                    LineNumber = lineNumber,
                    RawInstruction = line,
                    DestinationBits = GetDestinationBits(match.Groups["dest"].Value),
                    ComputationBits = computationBits,
                    JumpBits = GetJumpBits(match.Groups["jump"].Value),
                    IsAlternativeComputation = isAlternativeComp,
                }
            );
    }

    

    private ushort GetDestinationBits(string destinationString) =>
        (ushort)(
            (destinationString.Contains('A') ? 4 : 0)
            + (destinationString.Contains('D') ? 2 : 0)
            + (destinationString.Contains('M') ? 1 : 0)
        );

    private (ushort Bits, bool IsAlternativeComp) GetComputationBits(string value)
    {
        return value switch
        {
            "0"   => (0b101010, false),
            "1"   => (0b111111, false),
            "-1"  => (0b111010, false),
            "D"   => (0b001100, false),
            "A"   => (0b110000, false),
            "M"   => (0b110000, true),
            "!D"  => (0b001101, false),
            "!A"  => (0b110001, false),
            "!M"  => (0b110001, true),
            "-D"  => (0b001111, false),
            "-A"  => (0b110011, false),
            "-M"  => (0b110011, true),
            "D+1" => (0b011111, false),
            "A+1" => (0b110111, false),
            "M+1" => (0b110111, true),
            "D-1" => (0b001110, false),
            "A-1" => (0b110010, false),
            "M-1" => (0b110010, true),
            "D+A" => (0b000010, false),
            "D+M" => (0b000010, true),
            "D-A" => (0b010011, false),
            "D-M" => (0b010011, true),
            "A-D" => (0b000111, false),
            "M-D" => (0b000111, true),
            "D&A" => (0b000000, false),
            "D&M" => (0b000000, true),
            "D|A" => (0b010101, false),
            "D|M" => (0b010101, true),
            _ => throw new NotSupportedException($"The expression `{value}` is not supported")
        };
    }

    private ushort GetJumpBits(string value)
    {
        if (value.IsNullOrEmpty())
        {
            return 0;
        }

        return value switch
        {
            "JGT" => 0b001,
            "JEQ" => 0b010,
            "JGE" => 0b011,
            "JLT" => 0b100,
            "JNE" => 0b101,
            "JLE" => 0b110,
            "JMP" => 0b111,
            _ => throw new NotSupportedException($"The expression `{value}` is not supported")
        };
    }
}
