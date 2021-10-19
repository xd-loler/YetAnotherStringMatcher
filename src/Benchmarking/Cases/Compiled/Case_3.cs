using YetAnotherStringMatcher;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmarking.Cases.Compiled
{
    [MemoryDiagnoser]
    public class CaseCompiled_3
    {
        private Matcher matcher;
        private Regex regex;

        [GlobalSetup]
        public void GlobalSetup()
        {
            matcher = new Matcher()
                          .MatchAnything()
                          .Then("Apple")
                          .ThenDigitsOfLength(1)
                          .Build();

            regex = new Regex(@"Apple\d{1}", RegexOptions.Compiled);
        }

        [Params
        (
            "12312312312asdfsdfsdfdsApple3",
            "qqqqqqqqqZZZZZZApple45Watermelon",
            "xcvxcvcxvcxvcxvxcvxcvxcvxcvxc12312Watermelon"
        )]
        public string text;

        [Benchmark]
        public bool Case3_YASM()
        {
            return matcher.Check(text).Success;
        }

        [Benchmark]
        public bool Case3_Regex()
        {
            return regex.IsMatch(text);
        }
    }
}
