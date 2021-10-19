using YetAnotherStringMatcher;

namespace RegexCodeGeneration.Nodes
{
    internal class ThenNode : Node
    {
        public string Item { get; set; }

        public ThenNode(string item)
        {
            Item = item;
        }

        public ThenNode(string item, CheckOptions options) : this(item)
        {
            IsOptional = options.Optional;
            IgnoreCase = options.IgnoreCase;
        }

        public bool IsOptional { get; set; }

        public bool IgnoreCase { get; set; }

        public override string ToString()
        {
            return "Then Node";
        }
    }
}
