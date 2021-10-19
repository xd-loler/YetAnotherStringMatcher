using System;

namespace YetAnotherStringMatcher.Requirements
{
    public class ThenRequirement : IOperation
    {
        public string Name => "Match one element";

        public string Item { get; set; }

        public CheckOptions Options { get; set; } = new CheckOptions();

        public IOperation NextOperation { get; set; }

        public ThenRequirement(string item)
        {
            Item = item;
        }

        public CheckResult Check(string original, int index)
        {
            if (original is null)
                return new CheckResult(false, index);

            var strComparison = Options?.IgnoreCase ?? false ?
                                StringComparison.OrdinalIgnoreCase :
                                StringComparison.Ordinal;

            var matches = original.IndexOf(Item, index, strComparison) == index;

            if (matches)
            {
                if (NextOperation is null)
                {
                    return new CheckResult(true, index + Item.Length);
                }
                else
                {
                    var nextResult = NextOperation.Check(original, index + Item.Length);
                    return nextResult;
                }
            }
            else
            {
                return new CheckResult(this.Options.Optional, index);
            }
        }

        public override string ToString()
        {
            return $"Then Requirement '{Item}'";
        }
    }
}
