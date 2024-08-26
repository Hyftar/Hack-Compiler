namespace Compiler.Syntax;

public class AInstruction : Instruction
{
    public ushort Value { get; init; }

    public override string ToString() => $"@{this.Value}";

    public override ushort ToBinary() => (ushort)(this.Value & 0b0111111111111111);

    public override string ToBinaryString() => Convert.ToString(this.ToBinary(), 2).PadLeft(16, '0');
}
