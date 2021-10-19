using YetAnotherStringMatcher;
using System.Collections.Generic;

namespace RegexCodeGeneration.Nodes
{
    internal class SymbolsWithLenghtBetweenNode : Node
    {
        public List<string> Items { get; set; } = new List<string>();

        public SymbolsWithLenghtBetweenNode(List<string> items, int length)
        {
            Items = items;
            Length = length;
        }

        public SymbolsWithLenghtBetweenNode(List<string> items, int length, CheckOptions options) : this(items, length)
        {
            IsOptional = options.Optional;
            IgnoreCase = options.IgnoreCase;
        }

        public bool IsOptional { get; set; }

        public bool IgnoreCase { get; set; }

        public int Length { get; set; }

        public override string ToString()
        {
            return "SymbolsWithLenghtBetweenNode Node";
        }
    }
}
