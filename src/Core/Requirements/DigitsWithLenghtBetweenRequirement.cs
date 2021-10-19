using System;
using YetAnotherStringMatcher.Requirements.BaseClasses;

namespace YetAnotherStringMatcher.Requirements
{
    public class DigitsWithLenghtBetweenRequirement : SomethingOfLengthBetweenBase
    {
        public DigitsWithLenghtBetweenRequirement(int min, int max) : base(min, max)
        {
            Predicate = (char c, CheckOptions o) => char.IsDigit(c); ;
        }

        public override string Name => "Match digits that have length between [MIN...MAX]. " +
            "It prioritizes longest matches.";

        public Func<char, CheckOptions, bool> Predicate { get; }

        public override bool Satisfies(char c) => Predicate(c, Options);

        public override string ToString()
        {
            return "Digits With Lenght Between Requirement";
        }
    }
}
