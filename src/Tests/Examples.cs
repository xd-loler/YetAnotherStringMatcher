using Xunit;
using System.Linq;
using RegexCodeGeneration;
using YetAnotherStringMatcher;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Tests
{
    public class Examples
    {
        [Fact]
        public void Test000_PolishPostalCode()
        {
            // Polish Postal Code
            var input = "12-345";

            var result = new Matcher(input)
                             .MatchDigitsOfLength(2)
                             .Then("-")
                             .ThenDigitsOfLength(3)
                             .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test001_SamplePhoneNumbers()
        {
            // Sample Phone Number / Reusable Pattern
            var input = new List<string> { "+123 345 67 89", "+1424 345 67 89" };

            var pattern = new Matcher()
                              .Match("+")
                              .ThenDigitsOfLength(3)
                              .Then(" ")
                              .ThenDigitsOfLength(3)
                              .Then(" ")
                              .ThenDigitsOfLength(2)
                              .Then(" ")
                              .ThenDigitsOfLength(2);

            Assert.True(pattern.Check(input[0]).Success);
            Assert.False(pattern.Check(input[1]).Success);
        }

        // For docs purposes
        [Fact]
        public void Test001_Test()
        {
            var result = new Matcher("Apple Pineapple")
                            .Match("Apple ")
                            .Then("Coconut ").IsOptional()
                            .Then("Pineapple")
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test002_Test()
        {
            var input = "12-345";

            var matcher = new Matcher(input)
                             .MatchDigitsOfLength(2)
                             .Then("-")
                             .ThenDigitsOfLength(3);

            var generator = new RegexGenerator(matcher.GetRequirements.ToList());

            var result = generator.Emit();

            Assert.True(result.Success);
            Assert.False(string.IsNullOrWhiteSpace(result.Code));
            Assert.Equal(@"\d{2}(-)\d{3}", result.Code);

            var regex = new Regex(result.Code);

            Assert.True(matcher.Check(input).Success);
            Assert.Matches(regex, input);
        }

        [Fact]
        public void Test003_Test()
        {
            var inputs = new List<string> { "+1", "+12", "+123", "+1234" };

            var pattern = new Matcher()
                            .Match("+")
                            .ThenDigitsWithLengthBetween(1, 3)
                            .NoMore();

            var generator = new RegexGenerator(pattern.GetRequirements.ToList());

            var result = generator.Emit();

            Assert.True(result.Success);
            Assert.False(string.IsNullOrWhiteSpace(result.Code));
            Assert.Equal(@"(\+)\d{1,3}$", result.Code);

            var regex = new Regex(result.Code);

            Assert.True(pattern.Check(inputs[0]).Success);
            Assert.Matches(regex, inputs[0]);

            Assert.True(pattern.Check(inputs[1]).Success);
            Assert.Matches(regex, inputs[1]);

            Assert.True(pattern.Check(inputs[2]).Success);
            Assert.Matches(regex, inputs[2]);

            Assert.False(pattern.Check(inputs[3]).Success);
            Assert.DoesNotMatch(regex, inputs[3]);
        }

        [Fact]
        public void Test004_Backtracking()
        {
            var result = new Matcher("Apple Water_Banana Watermelon")
                             .Match("Apple")
                             .ThenAnything()
                             .Then("Water")
                             .Then("melon")
                             .Check();

            Assert.True(result.Success);
        }
    }
}
