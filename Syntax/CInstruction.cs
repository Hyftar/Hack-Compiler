namespace Compiler.Syntax;

public class CInstruction : Instruction
{
    public bool IsAlternativeComputation { get; set; }

    public ushort DestinationBits { get; init; }

    public ushort ComputationBits { get; init; }

    public ushort JumpBits { get; init; }

    public override string ToString() => throw new NotSupportedException();

    public override ushort ToBinary()
    {
        return
            (ushort)
            (
                (0b111 << 13)
                | (this.IsAlternativeComputation ? 1 : 0) << 12
                | (this.ComputationBits << 6)
                | (this.DestinationBits << 3)
                | (this.JumpBits)
            );

    }

    public override string ToBinaryString() => throw new NotImplementedException();
}
