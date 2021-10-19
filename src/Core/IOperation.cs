namespace YetAnotherStringMatcher
{
    public interface IOperation
    {
        /// <summary>
        /// User friendly name of this requirement.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// <para>
        /// Takes original string and current index
        /// e.g "abc" and 1
        /// and has to return whether current requirement is fulfilled and
        /// at which index should we move after this requirement.
        /// </para>
        /// <para>
        /// For example: We have string "abc" and current index: 2
        /// And our requirement tries to find string "c"
        /// Since "c" is at current index, then we return success and the next index: 3.
        /// The fact that it'll be out of bounds will be handled by evaluator.
        /// </para>
        /// </summary>
        /// <param name="original">Full, original string</param>
        /// <param name="index">Current index within original string which indicates start place for our requirement</param>
        /// <returns>Returns whether current requirement is fulfilled and at which next index we should move.</returns>
        CheckResult Check(string original, int index);

        CheckOptions Options { get; set; }

        IOperation NextOperation { get; set; }
    }
}