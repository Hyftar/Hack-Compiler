namespace Compiler.Filters;

public interface IGenericFilter
{
    IEnumerable<string> Filter(IEnumerable<string> input);
}
