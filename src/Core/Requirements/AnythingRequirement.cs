namespace YetAnotherStringMatcher.Requirements
{
    public class AnythingRequirement : IOperation
    {
        public AnythingRequirement()
        {
        }

        public AnythingRequirement(int expectedLength)
        {
            ExpectedLength = expectedLength;
        }

        public string Name => "Match some element(s)";

        public CheckOptions Options { get; set; } = new CheckOptions();

        public bool IsLastRequirement => NextOperation is null;

        public IOperation NextOperation { get; set; }

        private int? ExpectedLength { get; }

        public CheckResult Check(string original, int index)
        {
            if (original is null)
                return new CheckResult(false, index);

            var validIndex = IndexHelper.WithinBounds(original, index);

            if (!validIndex)
                return new CheckResult(this.Options.Optional, index);

            if (IsLastRequirement)
            {
                // Because the way we manipulate indices,
                // we're standing on last index and we haven't checked it yet,
                // but since it can be Anything, then we're fine.
                // With this approach "last index" is at LastIndex+1 (Length/Count)

                if (index == original.Length)
                {
                    return new CheckResult(false, index);
                }
                else
                {
                    return new CheckResult(true, original.Length);
                }
            }

            if (ExpectedLength.HasValue)
            {
                var aheadIndex = index + ExpectedLength.Value;

                if (!IndexHelper.WithinBounds(original, aheadIndex))
                {
                    return new CheckResult(false, index);
                }

                var nextResult = NextOperation.Check(original, aheadIndex);

                if (nextResult.Success)
                {
                    return new CheckResult(true, nextResult.NewIndex, nextResult);
                }
            }
            else
            {
                // + 1 because we want to match at least one character.
                for (int i = index + 1; i <= original.Length; i++)
                {
                    var nextResult = NextOperation.Check(original, i);

                    if (nextResult.Success)
                    {
                        return new CheckResult(true, nextResult.NewIndex, nextResult);
                    }
                }

                if (this.Options.Optional)
                {
                    var nextResult = NextOperation.Check(original, index);

                    if (nextResult.Success)
                    {
                        return new CheckResult(true, nextResult.NewIndex, nextResult);
                    }
                }
            }

            return new CheckResult(false, index);
        }

        public override string ToString()
        {
            return "Then Anything Requirement";
        }
    }
}
