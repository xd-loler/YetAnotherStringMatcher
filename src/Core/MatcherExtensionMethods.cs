using System;
using System.Linq;
using YetAnotherStringMatcher.Requirements;

namespace YetAnotherStringMatcher
{
    public static class MatcherExtensionMethods
    {
        // Basic

        public static Matcher Match(this Matcher matcher, string item)
        {
            return matcher.Then(item);
        }

        public static Matcher Then(this Matcher matcher, string item)
        {
            return matcher.ThenCustom(new ThenRequirement(item));
        }

        // AnyOf

        public static Matcher MatchAnyOf(this Matcher matcher, params string[] items)
        {
            return matcher.ThenCustom(new AnyOfRequirement(items));
        }

        public static Matcher ThenAnyOf(this Matcher matcher, params string[] items)
        {
            return matcher.ThenCustom(new AnyOfRequirement(items));
        }

        // Anything

        public static Matcher MatchAnything(this Matcher matcher)
        {
            return matcher.ThenCustom(new AnythingRequirement());
        }

        public static Matcher ThenAnything(this Matcher matcher)
        {
            return matcher.ThenCustom(new AnythingRequirement());
        }

        // Anything Of Length

        public static Matcher MatchAnythingOfLength(this Matcher matcher, int length)
        {
            return matcher.ThenCustom(new AnythingRequirement(length));
        }

        public static Matcher ThenAnythingOfLength(this Matcher matcher, int length)
        {
            return matcher.ThenCustom(new AnythingRequirement(length));
        }

        // Digits Of Length

        public static Matcher MatchDigitsOfLength(this Matcher matcher, int length)
        {
            return matcher.ThenCustom(new DigitsOfLenghtRequirement(length));
        }

        public static Matcher ThenDigitsOfLength(this Matcher matcher, int length)
        {
            return matcher.ThenCustom(new DigitsOfLenghtRequirement(length));
        }

        // Digits With Length Between

        public static Matcher MatchDigitsWithLengthBetween(this Matcher matcher, int min, int max)
        {
            return matcher.ThenCustom(new DigitsWithLenghtBetweenRequirement(min, max));
        }

        public static Matcher ThenDigitsWithLengthBetween(this Matcher matcher, int min, int max)
        {
            return matcher.ThenCustom(new DigitsWithLenghtBetweenRequirement(min, max));
        }

        // Symbols Of Length

        public static Matcher MatchSymbolsOfLength(this Matcher matcher, char[] symbols, int length)
        {
            return matcher.ThenCustom(new SymbolsOfLengthRequirement(length, symbols));
        }

        public static Matcher ThenSymbolsOfLength(this Matcher matcher, char[] symbols, int length)
        {
            return matcher.ThenCustom(new SymbolsOfLengthRequirement(length, symbols));
        }

        // Custom Of Length

        public static Matcher MatchCustomOfLength(this Matcher matcher, Func<char, CheckOptions, bool> pred, int length)
        {
            return matcher.ThenCustom(new SomethingOfLenghtRequirement(length, pred));
        }

        public static Matcher ThenCustomOfLength(this Matcher matcher, Func<char, CheckOptions, bool> pred, int length)
        {
            return matcher.ThenCustom(new SomethingOfLenghtRequirement(length, pred));
        }

        // Extract

        public static Matcher ExtractAs(this Matcher matcher, string key)
        {
            return matcher.ThenCustom(new ExtractOperation(key));
        }

        public static Matcher ThenExtractAs(this Matcher matcher, string key)
        {
            return matcher.ThenCustom(new ExtractOperation(key));
        }

        // Other:

        public static Matcher NoMore(this Matcher matcher)
        {
            return matcher.ThenCustom(new EndRequirement());
        }

        public static Matcher IgnoreCase(this Matcher matcher)
        {
            if (matcher.Operations.Count == 0)
                throw new Exception("Cannot apply Options when there's not even one Requirement registered.");

            matcher.Operations.Last().Options.IgnoreCase = true;

            return matcher;
        }

        public static Matcher IsOptional(this Matcher matcher)
        {
            if (matcher.Operations.Count == 0)
                throw new Exception("Cannot apply Options when there's not even one Requirement registered.");

            matcher.Operations.Last().Options.Optional = true;

            return matcher;
        }

        public static Matcher IgnoreCaseForAllExisting(this Matcher matcher)
        {
            if (matcher.Operations.Count == 0)
                throw new Exception("Cannot apply Options when there's not even one Requirement registered.");

            foreach (var req in matcher.Operations)
            {
                req.Options.IgnoreCase = true;
            }

            return matcher;
        }
    }
}
