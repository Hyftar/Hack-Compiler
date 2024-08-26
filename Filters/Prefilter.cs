namespace Compiler.Filters;

public interface IPrefilter : IGenericFilter
{
}

public class Prefilter : IPrefilter
{
    private readonly IList<IGenericFilter> filters = new List<IGenericFilter>();

    public Prefilter(
        IWhiteSpaceFilter whiteSpaceFilter)
    {
        this.filters.Add(whiteSpaceFilter);
    }

    public IEnumerable<string> Filter(IEnumerable<string> input)
    {
        var current = input;

        foreach (var filter in this.filters)
        {
            current = Apply(filter, current);
        }

        return current;
    }

    private static IEnumerable<string> Apply(IGenericFilter filter, IEnumerable<string> input)
    {
        return filter.Filter(input);
    }
}
