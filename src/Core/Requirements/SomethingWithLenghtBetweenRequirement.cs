using System;
using YetAnotherStringMatcher.Requirements.BaseClasses;

namespace YetAnotherStringMatcher.Requirements
{
    public class SomethingWithLenghtBetweenRequirement : SomethingOfLengthBetweenBase
    {
        public SomethingWithLenghtBetweenRequirement(int min, int max, Func<char, CheckOptions, bool> predicate)
            : base(min, max)
        {
            Predicate = predicate;
        }

        public override string Name => "Match something that matches predicate and has length between [MIN...MAX]. " +
            "It prioritizes longest matches.";

        public Func<char, CheckOptions, bool> Predicate { get; }

        public override bool Satisfies(char c) => Predicate(c, Options);

        public override string ToString()
        {
            return "Then Something With Lenght Between Requirement";
        }
    }
}
