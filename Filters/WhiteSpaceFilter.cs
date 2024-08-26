using HyftarCSExtensions.EnumerableExtensions;

namespace Compiler.Filters;

public interface IWhiteSpaceFilter : IGenericFilter { }

public class WhiteSpaceFilter : IWhiteSpaceFilter
{
    public IEnumerable<string> Filter(IEnumerable<string> input)
    {
        foreach (var (line, index) in input.WithIndex())
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var output = line.Replace(" ", "");

            var commentIndex = output.IndexOf("//");

            if (commentIndex != -1)
            {
                output = output.Substring(0, commentIndex);
            }

            if (output.Length > 0)
            {
                yield return output;
            }
        }
    }
}
