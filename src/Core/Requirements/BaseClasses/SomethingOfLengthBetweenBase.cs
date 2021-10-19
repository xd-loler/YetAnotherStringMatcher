using System.Linq;

namespace YetAnotherStringMatcher.Requirements.BaseClasses
{
    public abstract class SomethingOfLengthBetweenBase : IOperation
    {
        public virtual string Name => "Matches symbol(s) that do satisfy predicate " +
            "and their length is between [MIN...MAX]. It prioritizes longest matches.";

        public int MinimumLength { get; }

        public int MaximumLength { get; }

        public CheckOptions Options { get; set; } = new CheckOptions();

        public IOperation NextOperation { get; set; }

        public SomethingOfLengthBetweenBase(int min, int max)
        {
            MinimumLength = min;
            MaximumLength = max;
        }

        public CheckResult Check(string original, int index)
        {
            if (original is null)
                return new CheckResult(false, index);

            for (int currentOffset = MaximumLength; currentOffset >= MinimumLength; currentOffset--)
            {
                var aheadIndex = index + currentOffset - 1;

                if (!IndexHelper.WithinBounds(original, aheadIndex))
                {
                    continue;
                }

                var substr = original.Substring(index, currentOffset);

                var result = substr.ToCharArray().All(x => Satisfies(x));

                if (result)
                {
                    if (NextOperation is null)
                    {
                        return new CheckResult(result, result ? aheadIndex + 1 : index);
                    }
                    else
                    {
                        var nextResult = NextOperation.Check(original, aheadIndex + 1);
                        return nextResult;
                    }
                }
            }

            return new CheckResult(false, index);
        }

        public abstract bool Satisfies(char c);

        public override string ToString()
        {
            return "Then Something Of Length Base";
        }
    }
}
