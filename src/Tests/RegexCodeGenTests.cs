using Xunit;
using System.Linq;
using RegexCodeGeneration;
using YetAnotherStringMatcher;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tests
{
    public class RegexCodeGenTests
    {
        [Fact]
        public void Test001_ThenAnythingEmitter()
        {
            var text = "Apple";

            var matcher = new Matcher()
                              .ThenAnything();

            EnsureRegexAndMatcherReturnEqualResults(matcher, text);
        }

        [Fact]
        public void Test002_ThenAnythingEmitter_IgnoreCase()
        {
            var text = "APPLE";

            var matcher = new Matcher()
                              .ThenAnything().IgnoreCase();


            EnsureRegexAndMatcherReturnEqualResults(matcher, text);
        }

        [Fact]
        public void Test003_ThenAnythingEmitter_Optional()
        {
            var text = "";

            var matcher = new Matcher()
                              .ThenAnything().IsOptional();

            EnsureRegexAndMatcherReturnEqualResults(matcher, text);
        }

        [Fact]
        public void Test004_ThenAnythingEmitter_IgnoreCase_Optional()
        {
            var text = "";

            var matcher = new Matcher()
                             .ThenAnything().IgnoreCase().IsOptional();


            EnsureRegexAndMatcherReturnEqualResults(matcher, text);
        }

        [Fact]
        public void Test005_ThenAnyOf()
        {
            var text = "APPLE1";

            var matcher = new Matcher()
                              .MatchAnyOf("Apple", "Watermelon").IgnoreCase();


            EnsureRegexAndMatcherReturnEqualResults(matcher, text);
        }

        [Fact]
        public void Test006_NoMore()
        {
            var input = new List<string> { "Apple", "Apple1" };

            var matcher = new Matcher()
                              .ThenAnyOf("Apple")
                              .NoMore();

            EnsureRegexAndMatcherReturnEqualResults(matcher, input);
        }

        [Fact]
        public void Test007_Then()
        {
            var input = new List<string> { "Apple1", "Apple" };

            var matcher = new Matcher()
                              .ThenAnyOf("Apple")
                              .Then("1");

            EnsureRegexAndMatcherReturnEqualResults(matcher, input);
        }

        [Fact]
        public void Test008_SymbolsWithLenghtBetween()
        {
            var input = new List<string> { "Apple123", "Apple45" };

            var matcher = new Matcher()
                              .ThenAnyOf("Apple")
                              .ThenSymbolsOfLength(new[] { '1', '2', '3' }, 2);


            EnsureRegexAndMatcherReturnEqualResults(matcher, input);
        }

        [Fact]
        public void Test009_DigitsOfLength()
        {
            var input = new List<string> { "Apple123", "Apple45" };

            var matcher = new Matcher()
                              .ThenAnyOf("Apple")
                              .ThenDigitsOfLength(3);

            EnsureRegexAndMatcherReturnEqualResults(matcher, input);
        }

        [Fact]
        public void Test010_DigitsWithLengthBetween()
        {
            var input = new List<string> { "Apple123", "Apple5" };

            var matcher = new Matcher()
                                .ThenAnyOf("Apple")
                                .ThenDigitsWithLengthBetween(2,3);

            EnsureRegexAndMatcherReturnEqualResults(matcher, input);
        }

        [Fact]
        public void Test011_EscapeTest()
        {
            var text = "+Apple";

            var matcher = new Matcher()
                              .Match("+")
                              .MatchAnyOf("Apple", "Watermelon").IgnoreCase();

            EnsureRegexAndMatcherReturnEqualResults(matcher, text);
        }

        public static void EnsureRegexAndMatcherReturnEqualResults(Matcher matcher, string item)
        {
            matcher.Build();
            var generator = new RegexGenerator(matcher.GetRequirements.ToList());
            var emitted = generator.Emit();

            Assert.True(emitted.Success);
            Assert.False(string.IsNullOrWhiteSpace(emitted.Code));

            var regex = new Regex(emitted.Code);
            Assert.Equal(matcher.Check(item).Success, regex.IsMatch(item));
        }

        public static void EnsureRegexAndMatcherReturnEqualResults(Matcher matcher, List<string> input)
        {
            matcher.Build();
            var generator = new RegexGenerator(matcher.GetRequirements.ToList());
            var emitted = generator.Emit();

            Assert.True(emitted.Success);
            Assert.False(string.IsNullOrWhiteSpace(emitted.Code));

            var regex = new Regex(emitted.Code);

            foreach (var item in input)
            {
                Assert.Equal(matcher.Check(item).Success, regex.IsMatch(item));
            }
        }
    }
}
