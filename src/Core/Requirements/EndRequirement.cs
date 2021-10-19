namespace YetAnotherStringMatcher.Requirements
{
    public class EndRequirement : IOperation
    {
        public string Name => "Indicates that pattern has to end";

        public CheckOptions Options { get; set; } = new CheckOptions();

        public IOperation NextOperation { get; set; }

        public CheckResult Check(string original, int index)
        {
            if (original is null)
                return new CheckResult(false, index);

            if (index >= original.Length)
                return new CheckResult(true, index + 1);

            return new CheckResult(false, index);
        }

        public override string ToString()
        {
            return "End Requirement";
        }
    }
}
