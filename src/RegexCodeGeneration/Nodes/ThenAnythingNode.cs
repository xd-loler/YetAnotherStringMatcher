using YetAnotherStringMatcher;

namespace RegexCodeGeneration.Nodes
{
    internal class ThenAnythingNode : Node
    {
        public ThenAnythingNode()
        {

        }

        public ThenAnythingNode(CheckOptions options)
        {
            IsOptional = options.Optional;
            IgnoreCase = options.IgnoreCase;
        }

        public bool IsOptional { get; set; }

        public bool IgnoreCase { get; set; }

        public override string ToString()
        {
            return "ThenAnything Node";
        }
    }
}
