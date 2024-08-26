using AllDataScraper.IoC;
using Ninject;
using Ninject.Modules;

namespace Compiler.IoC;

internal class NInjectKernel
{
    public static IKernel Instance { get; private set; } = new StandardKernel(ResolveIoCModules().ToArray());

    private static IEnumerable<INinjectModule> ResolveIoCModules()
    {
        yield return new CompilerIoCModule();
    }
}
