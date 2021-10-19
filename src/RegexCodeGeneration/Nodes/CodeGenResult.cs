namespace RegexCodeGeneration.Nodes
{
    public class CodeGenResult
    {
        private CodeGenResult() { }

        public bool Success { get; set; }

        public string Code { get; set; }

        public string Error { get; set; }

        public static CodeGenResult ErrorResult(string s)
        {
            return new CodeGenResult
            {
                Success = false,
                Error = s,
                Code = ""
            };
        }

        public static CodeGenResult SuccessResult(string code)
        {
            return new CodeGenResult
            {
                Success = true,
                Error = "",
                Code = code
            };
        }
    }
}
