using System;
using Xunit;
using YetAnotherStringMatcher;
using System.Collections.Generic;

namespace Tests
{
    public class BasicTests
    {
        [Fact]
        public void Test000_NoRequirements()
        {
            var result = new Matcher("abc_aBc").Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test001_Then()
        {
            var matcher = new Matcher("123_123x")
                            .Match("123")
                            .Then("_")
                            .Then("123")
                            .Check();

            Assert.True(matcher.Success);
        }

        [Fact]
        public void Test002_ThenAnyOf()
        {
            var matcher = new Matcher("123_123")
                              .Match("123")
                              .ThenAnyOf("123", "_", "_1", "_12")
                              .Check();

            Assert.True(matcher.Success);
        }

        [Fact]
        public void Test004_IgnoreCaseOption()
        {
            var result = new Matcher("abc_aBc")
                            .Match("abc")
                            .ThenAnyOf("_", "_aE", "_ABC").IgnoreCase()
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test005_ThenAnyOf_Fail()
        {
            var result = new Matcher("abc_aBc")
                            .Match("abc")
                            .ThenAnyOf("_aBC", "_Ab", "_A", "_ABC")
                            .Check();

            Assert.False(result.Success);
        }

        [Fact]
        public void Test006_ThenAnything()
        {
            var result = new Matcher("abc_aBc")
                            .Match("abc")
                            .ThenAnything()
                            .Then("c")
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test007_ThenAnythingOfLength()
        {
            var result = new Matcher("abc12c")
                            .Match("abc")
                            .ThenAnythingOfLength(2)
                            .Then("c")
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test008_ThenDigitsOfLength()
        {
            var result = new Matcher("abc12c")
                            .Match("abc")
                            .ThenDigitsOfLength(2)
                            .Then("c")
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test009_ThenAnything_v2()
        {
            var result = new Matcher("abc_123aBcQQQQQQQQQQQQ")
                            .Match("abc")
                            .ThenAnything()
                            .Then("c")
                            .ThenAnything()
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test010_ThenAnyOf()
        {
            var matcher = new Matcher("a")
                            .ThenAnyOf("aa")
                            .Check();

            Assert.False(matcher.Success);
        }

        [Fact]
        public void Test011_ThenSymbols()
        {
            var matcher = new Matcher("abc12c")
                            .Match("abc")
                            .ThenSymbolsOfLength("abc12345".ToCharArray(), 2)
                            .Then("c")
                            .Check();

            Assert.True(matcher.Success);
        }

        [Fact]
        public void Test012_ThenSymbols()
        {
            var matcher = new Matcher("abc12c")
                            .Match("abc")
                            .ThenSymbolsOfLength("abc1345".ToCharArray(), 20)
                            .Then("c")
                            .Check();

            Assert.False(matcher.Success);
        }

        [Fact]
        public void Test013_ThenCustomOfLength()
        {
            Func<char, CheckOptions, bool> pred1 =
                (char c, CheckOptions o) => c == '1';

            Func<char, CheckOptions, bool> pred2 =
                (char c, CheckOptions o) => c == '3';

            var matcher = new Matcher("123")
                            .ThenCustomOfLength(pred1, 1)
                            .ThenAnything()
                            .ThenCustomOfLength(pred2, 1)
                            .Check();

            Assert.True(matcher.Success);
        }

        [Fact]
        public void Test014_ThenAnythingOfLength()
        {
            var result = new Matcher("abc12c")
                            .Match("abc")
                            .ThenAnythingOfLength(3)
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test015_ThenSymbols_WithoutIgnoreCase_Fails()
        {
            var matcher = new Matcher("ABC12c")
                            .ThenSymbolsOfLength("cb12345a".ToCharArray(), 6)
                            .Check();

            Assert.False(matcher.Success);
        }

        [Fact]
        public void Test016_ThenSymbols_IgnoreCase()
        {
            var matcher = new Matcher("ABC12c")
                            .ThenSymbolsOfLength("cb12345a".ToCharArray(), 6)
                            .IgnoreCase()
                            .Check();

            Assert.True(matcher.Success);
        }

        [Fact]
        public void Test017_EndRequirement_Fails()
        {
            var result = new Matcher("abc_c")
                            .Match("abc")
                            .NoMore()
                            .Check();

            Assert.False(result.Success);
        }

        [Fact]
        public void Test018_EndRequirement()
        {
            var result = new Matcher("abc_")
                            .Match("abc")
                            .ThenAnything()
                            .NoMore()
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test019_BadEndRequirement()
        {
            var result = new Matcher("abc_")
                            .Match("abc_")
                            .NoMore()
                            .NoMore();

            Assert.Throws<InvalidOperationException>(() => result.Check());
        }

        [Fact]
        public void Test020_BadEndRequirement()
        {
            var result = new Matcher("abc_1")
                            .Match("abc_")
                            .NoMore()
                            .Then("1");

            Assert.Throws<InvalidOperationException>(() => result.Check());
        }

        [Fact]
        public void Test021_DigitsWithLengthBetween()
        {
            var inputs = new List<string> { "+1", "+12", "+123", "+1234" };

            var pattern = new Matcher()
                            .Match("+")
                            .ThenDigitsWithLengthBetween(1, 3)
                            .NoMore();

            Assert.True(pattern.Check(inputs[0]).Success);
            Assert.True(pattern.Check(inputs[1]).Success);
            Assert.True(pattern.Check(inputs[2]).Success);

            Assert.False(pattern.Check(inputs[3]).Success);
        }

        [Fact]
        public void Test022_ThenAnything_EdgeCase()
        {
            var result = new Matcher("12")
                            .Match("1")
                            .ThenAnything()
                            .Then("2")
                            .Check();

            Assert.False(result.Success);
        }

        [Fact]
        public void Test023_ThenAnything()
        {
            var result = new Matcher("12")
                            .Match("1")
                            .ThenAnything()
                            .Then("2")
                            .Then("2")
                            .Check();

            Assert.False(result.Success);
        }

        [Fact]
        public void Test024_Optional()
        {
            var result = new Matcher("13")
                            .Match("1")
                            .Then("2").IsOptional()
                            .Then("3")
                            .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test025_ThenAnyOf()
        {
            var input = new List<string>
            {
                "[2021-09-05] ERROR: Message1",
                "[2021-09-05] WARNING: Message1",
                "[2021-09-07] WARNING: Message1",
            };

            var pattern = new Matcher()
                              .Match("[2021-09-05] ")
                              .ThenAnyOf("WARNING:", "ERROR:");

            Assert.True(pattern.Check(input[0]).Success);
            Assert.True(pattern.Check(input[1]).Success);

            Assert.False(pattern.Check(input[2]).Success);
        }

        [Fact]
        public void Test026_Email_Like()
        {
            var input = new List<string>
            {
                "test@@.",
                "test@gmail.com",
                "\"test\"@gmail.com",
                "test@ab.c",
            };

            var pattern = new Matcher()
                          .MatchAnything()
                          .Then("@")
                          .ThenAnything()
                          .Then(".")
                          .ThenAnything();

            Assert.False(pattern.Check(input[0]).Success);
            Assert.True(pattern.Check(input[1]).Success);
            Assert.True(pattern.Check(input[2]).Success);
            Assert.True(pattern.Check(input[3]).Success);
        }

        [Fact]
        public void Test027_Email_Like()
        {
            var pattern = new Matcher("1@@..")
                          .MatchAnything()
                          .Then("@")
                          .ThenAnything()
                          .Then(".")
                          .ThenAnything();

            Assert.True(pattern.Check().Success);
        }

        [Fact]
        public void Test028_AFew_ThenAnything()
        {
            var pattern = new Matcher("123")
                              .MatchAnything()
                              .ThenAnything()
                              .ThenAnything()
                              .Check();

            Assert.True(pattern.Success);
        }

        [Fact]
        public void Test029_AFew_ThenAnything()
        {
            var pattern = new Matcher("123456")
                              .MatchAnything()
                              .ThenAnything()
                              .ThenAnything()
                              .ThenAnything()
                              .ThenAnything()
                              .ThenAnything()
                              .Check();

            Assert.True(pattern.Success);
        }

        [Fact]
        public void Test030_OptionalAnything()
        {
            var input = new List<string>
            {
                "Parcel Delivery London",
                "ParcelLondon",
                "Letter Delivery London",
            };

            var pattern = new Matcher()
                              .MatchAnyOf("Parcel", "Taxi")
                              .ThenAnything().IsOptional()
                              .ThenAnyOf("London", "Zurich");

            Assert.True(pattern.Check(input[0]).Success);
            Assert.True(pattern.Check(input[1]).Success);
            Assert.False(pattern.Check(input[2]).Success);
        }

        [Fact]
        public void Test031_Extract()
        {
            var str = @"RROR: duplicate key value violates unique...
                        Detail: Key (some_column)=(b01a0e23-da71-3a08-9893-11b8b2dfb069) already exists.";

            var check = new Matcher(str)
                            .MatchAnything()
                            .Then("duplicate key value violates unique")
                            .ThenAnything()
                            .Then("Detail: Key ")
                            .ThenExtractAs("output")
                            .Then(" already exists.")
                            .Check();

            Assert.True(check.Success);
            Assert.Equal("(some_column)=(b01a0e23-da71-3a08-9893-11b8b2dfb069)", check.ExtractedData["output"]);
        }

        [Fact]
        public void Test032_Extract()
        {
            var str = @"abc";

            var check = new Matcher(str)
                            .Match("a")
                            .ThenExtractAs("output")
                            .ThenAnything()
                            .Check();

            Assert.True(check.Success);
            Assert.Equal("b", check.ExtractedData["output"]);
        }

        [Fact]
        public void Test033_Extract()
        {
            var str = @"abc";

            var check = new Matcher(str)
                            .ThenExtractAs("output")
                            .Then("c")
                            .Check();

            Assert.True(check.Success);
            Assert.Equal("ab", check.ExtractedData["output"]);
        }

        [Fact]
        public void Test034_IndexOutOfRange()
        {
            var str = @"abc";

            var check = new Matcher(str)
                            .ThenExtractAs("output")
                            .Then("cc")
                            .Check();

            Assert.False(check.Success);
        }

        [Fact]
        public void Test035_Backtracking1()
        {
            var result = new Matcher("a bc bd")
                             .Match("a")
                             .ThenAnything()
                             .Then("b")
                             .Then("d")
                             .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test036_Backtracking2()
        {
            var result = new Matcher("ab")
                             .MatchAnyOf("a", "ab")
                             .ThenAnyOf("b", "c")
                             .Check();

            Assert.True(result.Success);
        }

        [Fact]
        public void Test037_Backtracking3()
        {
            var result = new Matcher("abq")
                             .MatchAnyOf("a", "ab")
                             .ThenAnyOf("b", "c")
                             .Then("x")
                             .Check();

            Assert.False(result.Success);
        }
    }
}
