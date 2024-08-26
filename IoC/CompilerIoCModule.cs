using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Compiler.Parsers;
using Compiler.Syntax;

namespace AllDataScraper.IoC
{
    internal class CompilerIoCModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(
                x =>
                    x.FromThisAssembly()
                     .IncludingNonPublicTypes()
                     .SelectAllClasses()
                     .BindDefaultInterface()
            );

            this.Unbind<ISymbolsTable>();
            this.Bind<ISymbolsTable>().To<SymbolsTable>().InSingletonScope();
        }
    }
}
