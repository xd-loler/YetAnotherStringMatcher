using System;
using System.Linq;
using System.Collections.Generic;
using YetAnotherStringMatcher.Requirements.BaseClasses;

namespace YetAnotherStringMatcher.Requirements
{
    public class SymbolsOfLengthRequirement : SomethingOfLengthBase
    {
        public SymbolsOfLengthRequirement(int length, IEnumerable<char> symbols) : base(length)
        {
            Symbols = symbols.Select(x => x.ToString()).ToList();
        }

        public override string Name => "Match symbols(s) of expected length";

        public List<string> Symbols { get; } = new List<string>();

        public override bool Satisfies(char c)
        {
            var strComparison = Options?.IgnoreCase ?? false ?
                                StringComparison.OrdinalIgnoreCase :
                                StringComparison.Ordinal;

            var str = c.ToString();

            return Symbols.Any(x => x.Equals(str, strComparison));
        }

        public override string ToString()
        {
            return "Then Symbols Of Length Requirement";
        }
    }
}
