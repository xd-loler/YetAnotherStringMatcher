using YetAnotherStringMatcher;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmarking.Cases.Compiled
{
    [MemoryDiagnoser]
    public class CaseCompiled_4
    {
        private Matcher matcher;
        private Regex regex;

        [GlobalSetup]
        public void GlobalSetup()
        {
            matcher = new Matcher()
                          .MatchAnyOf("Parcel", "Taxi")
                          .ThenAnything().IsOptional()
                          .ThenAnyOf("London", "Zurich")
                          .Build();

            regex = new Regex("^(Parcel|Taxi).*?(London|Zurich)", RegexOptions.Compiled);
        }

        [Params
        (
            "Parcel Delivery London",
            "ParcelLondon",
            "Letter Delivery London"
        )]
        public string text;

        [Benchmark]
        public bool Case4_YASM()
        {
            return matcher.Check(text).Success;
        }

        [Benchmark]
        public bool Case4_Regex()
        {
            return regex.IsMatch(text);
        }
    }
}
