using BenchmarkDotNet.Running;
using Tests;

namespace Benchmarking
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkSwitcher.FromAssembly(typeof(BasicTests).Assembly).RunAll();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
