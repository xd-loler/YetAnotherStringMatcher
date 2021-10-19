using System.Collections.Generic;

namespace YetAnotherStringMatcher
{
    public class EvaluationResult
    {
        public EvaluationResult(bool success, string details)
        {
            Success = success;
            Details = details;
        }

        public EvaluationResult(bool success, string details, Dictionary<string, string> extractedData)
        {
            Success = success;
            Details = details;
            ExtractedData = extractedData;
        }

        public bool Success { get; }

        public string Details { get; }

        public Dictionary<string, string> ExtractedData { get; set; } = new Dictionary<string, string>();

        public override string ToString()
        {
            return $"Success? {Success} ErrorMessage? \"{Details}\"";
        }
    }
}
