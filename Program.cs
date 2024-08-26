using Compiler.IoC;
using Ninject;

namespace Compiler;

public class Program
{
    public async static Task Main(string[] args)
    {
        var kernel = NInjectKernel.Instance;

        var parser = kernel.Get<IParser>();

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide an assembly file name");

            return;
        }

        var filePath = args[0];
        var outputFilePath = filePath;

        if (outputFilePath.EndsWith(".asm"))
        {
            outputFilePath = filePath[..-4] + ".hack";
        }
        else
        {
            filePath = filePath + ".asm";
            outputFilePath = outputFilePath + ".hack";
        }

        await parser.Parse(filePath, outputFilePath);
    }
}