using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;
using YetAnotherStringMatcher;

namespace Benchmarking.Cases.Compiled
{
    [MemoryDiagnoser]
    public class CaseCompiled_1
    {
        private Matcher matcher;
        private Regex regex;

        [GlobalSetup]
        public void GlobalSetup()
        {
            matcher = new Matcher()
                          .Match("Apple")
                          .Build();

            regex = new Regex("^Apple", RegexOptions.Compiled);
        }

        [Params("Apple", "AppleWatermelon", "Watermelon")]
        public string text;

        [Benchmark]
        public bool Case1_YASM()
        {
            return matcher.Check(text).Success;
        }

        [Benchmark]
        public bool Case1_Regex()
        {
            return regex.IsMatch(text);
        }
    }
}
