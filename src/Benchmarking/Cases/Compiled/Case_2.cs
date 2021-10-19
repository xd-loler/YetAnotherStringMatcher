using YetAnotherStringMatcher;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmarking.Cases.Compiled
{
    [MemoryDiagnoser]
    public class CaseCompiled_2
    {
        private Matcher matcher;
        private Regex regex;

        [GlobalSetup]
        public void GlobalSetup()
        {
            matcher = new Matcher()
                          .MatchAnything()
                          .Then("Apple")
                          .Build();

            regex = new Regex("^Apple", RegexOptions.Compiled);
        }

        [Params
        (
            "12312312312asdfsdfsdfdsApple",
            "qqqqqqqqqZZZZZZAppleWatermelon",
            "xcvxcvcxvcxvcxvxcvxcvxcvxcvxc12312Watermelon"
        )]
        public string text;

        [Benchmark]
        public bool Case2_YASM()
        {
            return matcher.Check(text).Success;
        }

        [Benchmark]
        public bool Case2_Regex()
        {
            return regex.IsMatch(text);
        }
    }
}
