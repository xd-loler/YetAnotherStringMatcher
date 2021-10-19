using System.Text;

namespace YetAnotherStringMatcher.Requirements
{
    public class ExtractOperation : IOperation
    {
        public ExtractOperation()
        {

        }

        public ExtractOperation(int expectedLength)
        {
            ExpectedLength = expectedLength;
        }

        public ExtractOperation(string extractAs)
        {
            ExtractAs = extractAs;
        }

        public ExtractOperation(int expectedLength, string extractAs)
        {
            ExtractAs = extractAs;
            ExpectedLength = expectedLength;
        }

        public string Name => "Extract some element(s)";

        public CheckOptions Options { get; set; } = new CheckOptions();

        public bool IsLastRequirement => NextOperation is null;

        public IOperation NextOperation { get; set; }

        private int? ExpectedLength { get; }

        private string ExtractAs { get; }

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
                    return new CheckResult(true, original.Length, ExtractAs, original[original.Length - 1].ToString());
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
                    var substr = original.Substring(index, ExpectedLength.Value);
                    return new CheckResult(true, nextResult.NewIndex, ExtractAs, substr, nextResult);
                }
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append(original[index]);

                // + 1 because we want to match at least one character.
                for (int i = index + 1; i <= original.Length; i++)
                {
                    if (IndexHelper.WithinBounds(original, i))
                    {
                        sb.Append(original[i]);
                    }

                    var nextResult = NextOperation.Check(original, i);

                    if (nextResult.Success)
                    {
                        var extracted_string = sb.ToString();

                        if (extracted_string.Length > 0)
                            extracted_string = extracted_string.Remove(extracted_string.Length - 1, 1);

                        return new CheckResult(true, nextResult.NewIndex, ExtractAs, extracted_string, nextResult);
                    }
                }

                if (this.Options.Optional)
                {
                    var nextResult = NextOperation.Check(original, index);

                    if (nextResult.Success)
                    {
                        return new CheckResult(true, nextResult.NewIndex, ExtractAs, "", nextResult);
                    }
                }
            }

            return new CheckResult(false, index);
        }

        public override string ToString()
        {
            return "Then Extract Operation";
        }
    }
}
