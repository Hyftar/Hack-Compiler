namespace Compiler.Syntax;

public abstract class Instruction
{
    public int LineNumber { get; init; }

    public string RawInstruction {  get; init; }

    public abstract override string ToString();

    public abstract ushort ToBinary();

    public abstract string ToBinaryString();
}
