namespace YetAnotherStringMatcher
{
    public static class IndexHelper
    {
        public static bool WithinBounds(string str, int index)
        {
            return index < str.Length && index >= 0;
        }
    }
}
