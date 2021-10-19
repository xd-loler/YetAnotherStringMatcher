using YetAnotherStringMatcher;

namespace RegexCodeGeneration.Nodes
{
    internal class DigitsWithLenghtBetweenNode : Node
    {
        public DigitsWithLenghtBetweenNode(int minimum, int maximum)
        {
            MinimumLength = minimum;
            MaximumLength = maximum;
        }

        public DigitsWithLenghtBetweenNode(int minimum, int maximum, CheckOptions options) 
            : this(minimum, maximum)
        {
            IsOptional = options.Optional;
            IgnoreCase = options.IgnoreCase;
        }

        public bool IsOptional { get; set; }

        public bool IgnoreCase { get; set; }

        public int MinimumLength { get; }

        public int MaximumLength { get; }

        public override string ToString()
        {
            return "Digits With Lenght Between Node";
        }
    }
}
