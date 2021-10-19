using System;
using System.Linq;

namespace YetAnotherStringMatcher.Requirements
{
    public class AnyOfRequirement : IOperation
    {
        public string Name => "Match one element that's also an element of provided Items list";

        public string[] Items { get; set; }

        public CheckOptions Options { get; set; } = new CheckOptions();

        public IOperation NextOperation { get; set; }

        public AnyOfRequirement(params string[] items)
        {
            Items = items;
        }

        public CheckResult Check(string original, int index)
        {
            if (original is null)
                return new CheckResult(false, index);

            var strComparison = Options?.IgnoreCase ?? false ?
                                StringComparison.OrdinalIgnoreCase :
                                StringComparison.Ordinal;

            foreach (var item in Items)
            {
                if (original.IndexOf(item, index, strComparison) == index)
                {
                    if (NextOperation is null)
                    {
                        return new CheckResult(true, index + item.Length);
                    }
                    else
                    {
                        var nextResult = NextOperation.Check(original, index + item.Length);

                        if (nextResult.Success)
                            return nextResult;
                    }
                }
            }

            return new CheckResult(false, index);
        }

        public override string ToString()
        {
            return "Then Any Of Requirement";
        }
    }
}
