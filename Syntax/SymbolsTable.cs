namespace Compiler.Syntax;

public interface ISymbolsTable
{
    void AddLabel(string label, ushort lineReference);

    ushort GetReference(string name);
}

public class SymbolsTable : ISymbolsTable
{
    private readonly Dictionary<string, ushort> symbols =
        new() // pre-defined symbols
        {
            { "R0",    0 },
            { "R1",    1 },
            { "R2",    2 },
            { "R3",    3 },
            { "R4",    4 },
            { "R5",    5 },
            { "R6",    6 },
            { "R7",    7 },
            { "R8",    8 },
            { "R9",    9 },
            { "R10",  10 },
            { "R11",  11 },
            { "R12",  12 },
            { "R13",  13 },
            { "R14",  14 },
            { "R15",  15 },
            { "SP",    0 },
            { "LCL",   1 },
            { "ARG",   2 },
            { "THIS",  3 },
            { "THAT",  4 },
            { "KBD", 24576 },
            { "SCREEN", 16384 },
        };

    private ushort currentVariablesIndex = 16;

    public void AddLabel(string label, ushort lineReference)
    {
        if (!this.symbols.TryAdd(label, lineReference))
        {
            throw new InvalidOperationException($"Label {label} already exists with address {symbols[label]}");
        }
    }

    public ushort GetReference(string name)
    {
        if (this.symbols.TryAdd(name, this.currentVariablesIndex))
        {
            return this.currentVariablesIndex++;
        }

        return this.symbols[name];
    }
}
