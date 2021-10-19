using YetAnotherStringMatcher;

namespace RegexCodeGeneration.Nodes
{
    internal class DigitsOfLengthNode : Node
    {
        public DigitsOfLengthNode(int length)
        {
            Length = length;
        }

        public DigitsOfLengthNode(int length, CheckOptions options) : this(length)
        {
            IsOptional = options.Optional;
            IgnoreCase = options.IgnoreCase;
        }

        public bool IsOptional { get; set; }

        public bool IgnoreCase { get; set; }

        public int Length { get; }

        public override string ToString()
        {
            return "Digits Of Lenght Between Node";
        }
    }
}
