using YetAnotherStringMatcher;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmarking.Cases.NotCompiled
{
    [MemoryDiagnoser]
    public class CaseNotCompiled_3
    {
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
            return new Matcher(text)
                       .MatchAnything()
                       .Then("Apple")
                       .ThenDigitsOfLength(1)
                       .Check()
                       .Success;
        }

        [Benchmark]
        public bool Case3_Regex()
        {
            return new Regex(@"Apple\d{1}").IsMatch(text);
        }
    }
}
