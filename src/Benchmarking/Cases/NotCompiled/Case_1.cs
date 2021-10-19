using YetAnotherStringMatcher;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmarking.Cases.NotCompiled
{
    [MemoryDiagnoser]
    public class CaseNotCompiled_1
    {
        [Params("Apple", "AppleWatermelon", "Watermelon")]
        public string text;

        [Benchmark]
        public bool Case1_YASM()
        {
            return new Matcher(text).Match("Apple").Check().Success;
        }

        [Benchmark]
        public bool Case1_Regex()
        {
            return new Regex("^Apple").IsMatch(text);
        }
    }
}
