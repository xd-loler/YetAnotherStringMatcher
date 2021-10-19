using System;
using YetAnotherStringMatcher.Requirements.BaseClasses;

namespace YetAnotherStringMatcher.Requirements
{
    public class SomethingOfLenghtRequirement : SomethingOfLengthBase
    {
        public SomethingOfLenghtRequirement(int length, Func<char, CheckOptions, bool> predicate)
            : base(length)
        {
            Predicate = predicate;
        }

        public override string Name => "Match something that matches predicate and has expected length";

        public Func<char, CheckOptions, bool> Predicate { get; }

        public override bool Satisfies(char c) => Predicate(c, Options);

        public override string ToString()
        {
            return "Then Something Of Length Requirement";
        }
    }
}
