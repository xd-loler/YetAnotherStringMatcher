using YetAnotherStringMatcher;
using System.Collections.Generic;

namespace RegexCodeGeneration.Nodes
{
    internal class AnyOfNode : Node
    {
        public List<string> Items { get; set; } = new List<string>();

        public AnyOfNode(List<string> items)
        {
            Items = items;
        }

        public AnyOfNode(List<string> items, CheckOptions options) : this(items)
        {
            IsOptional = options.Optional;
            IgnoreCase = options.IgnoreCase;
        }

        public bool IsOptional { get; set; }

        public bool IgnoreCase { get; set; }

        public override string ToString()
        {
            return "AnyOfNode Node";
        }
    }
}
