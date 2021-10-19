using System.Linq;

namespace YetAnotherStringMatcher.Requirements.BaseClasses
{
    public abstract class SomethingOfLengthBase : IOperation
    {
        public virtual string Name => "Matches symbol(s) that do satisfy predicate " +
            "and have expected length";

        public int Length { get; }

        public CheckOptions Options { get; set; } = new CheckOptions();

        public IOperation NextOperation { get; set; }

        public SomethingOfLengthBase(int length)
        {
            Length = length;
        }

        public CheckResult Check(string original, int index)
        {
            if (original is null)
                return new CheckResult(false, index);

            var aheadIndex = index + Length - 1;

            if (!IndexHelper.WithinBounds(original, aheadIndex))
            {
                return new CheckResult(false, index);
            }

            var substr = original.Substring(index, Length);

            var result = substr.ToCharArray().All(x => Satisfies(x));

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

        public abstract bool Satisfies(char c);

        public override string ToString()
        {
            return "Then Something Of Length Base";
        }
    }
}
