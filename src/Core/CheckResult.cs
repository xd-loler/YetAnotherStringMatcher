using System.Collections.Generic;

namespace YetAnotherStringMatcher
{
    public class CheckResult
    {
        public CheckResult(bool success, int newIndex)
        {
            Success = success;
            NewIndex = newIndex;
        }

        public CheckResult(bool success, int newIndex, string key, string value)
        {
            Success = success;
            NewIndex = newIndex;
            ExtractedData.Add(key, value);
        }

        public CheckResult(bool success, int newIndex, CheckResult result) : this(success, newIndex)
        {
            foreach (var entry in result.ExtractedData)
                ExtractedData.Add(entry.Key, entry.Value);
        }

        public CheckResult(bool success, int newIndex, string key, string value, CheckResult result) : this(success, newIndex, key, value)
        {
            foreach (var entry in result.ExtractedData)
                ExtractedData.Add(entry.Key, entry.Value);
        }

        public bool Success { get; set; }

        public int NewIndex { get; set; }

        public Dictionary<string, string> ExtractedData = new Dictionary<string, string>();

        public override string ToString()
        {
            return $"Success? {Success} Index? {NewIndex}";
        }
    }
}
