using YetAnotherStringMatcher;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmarking.Cases.NotCompiled
{
    [MemoryDiagnoser]
    public class CaseNotCompiled_4
    {
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
            return new Matcher(text)
                       .MatchAnyOf("Parcel", "Taxi")
                       .ThenAnything().IsOptional()
                       .ThenAnyOf("London", "Zurich")
                       .Check()
                       .Success;
        }

        [Benchmark]
        public bool Case4_Regex()
        {
            return new Regex("^(Parcel|Taxi).*?(London|Zurich)").IsMatch(text);
        }
    }
}
