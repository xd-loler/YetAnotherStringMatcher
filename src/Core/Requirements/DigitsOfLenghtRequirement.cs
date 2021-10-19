using System;
using YetAnotherStringMatcher.Requirements.BaseClasses;

namespace YetAnotherStringMatcher.Requirements
{
    public class DigitsOfLenghtRequirement : SomethingOfLengthBase
    {
        public DigitsOfLenghtRequirement(int length)
            : base(length)
        {
            Predicate = (char c, CheckOptions o) => char.IsDigit(c);
        }

        public override string Name => "Match digits that have expected length";

        public Func<char, CheckOptions, bool> Predicate { get; }

        public override bool Satisfies(char c) => Predicate(c, Options);

        public override string ToString()
        {
            return "Digits Of Lenght Requirement";
        }
    }
}
