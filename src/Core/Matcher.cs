using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YetAnotherStringMatcher.Requirements;

namespace YetAnotherStringMatcher
{
    public class Matcher
    {
        internal List<IOperation> Operations { get; set; } = new List<IOperation>();

        internal IOperation Root { get; set; }

        internal IOperation Last { get; set; }

        public string OriginalString { get; private set; }

        /// <summary>
        /// Prevents unnecessary Prepare() recalculation
        /// All Operations should be added via ThenCustom since it invalidates IsPrepared
        /// </summary>
        private bool IsPrepared { get; set; }

        public Matcher(string original_string)
        {
            OriginalString = original_string;
        }

        public Matcher()
        {
        }

        // Public API here and inside MatcherExtensionMethods.cs

        public ReadOnlyCollection<IOperation> GetRequirements => new ReadOnlyCollection<IOperation>(Operations);

        public EvaluationResult Check()
        {
            if (!IsPrepared)
            {
                Prepare();
            }

            if (OriginalString is null)
                throw new ArgumentNullException("Input string cannot be NULL.");

            if (Root is null)
                return new EvaluationResult(true, "");

            return Evaluate();
        }

        public EvaluationResult Check(string input)
        {
            OriginalString = input;
            return Check();
        }

        /// <summary>
        /// Method which allows to add another requirement to the list
        /// </summary>
        /// <param name="requirement"></param>
        public Matcher ThenCustom(IOperation requirement)
        {
            AddNewOperation(requirement);
            return this;
        }

        public Matcher Build()
        {
            Prepare();
            return this;
        }

        // Internal Stuff/APIs

        private void AddNewOperation(IOperation requirement)
        {
            if (Root is null)
            {
                Root = requirement;
                Last = Root;
            }
            else
            {
                Last.NextOperation = requirement;
                Last = Last.NextOperation;
            }

            Operations.Add(requirement);
            IsPrepared = false;
        }

        private EvaluationResult Evaluate()
        {
            var result = Root.Check(OriginalString, 0);

            if (result.Success)
            {
                return new EvaluationResult(true, "", result.ExtractedData);
            }
            else
            {
                // TODO: Improve error diagnostics
                return new EvaluationResult(false, $"Some requirement wasn't fulfilled.");
            }
        }

        private void Prepare()
        {
            var endRequirements = Operations
                .Select((x, Index) => new { Object = x, Index })
                .Where(data => data.Object is EndRequirement)
                .Select(x => x.Index)
                .ToList();

            if (endRequirements.Any() &&
                (endRequirements.Count > 1 || endRequirements[0] != Operations.Count - 1))
            {
                throw new InvalidOperationException("NoMore can occur only once at the end of pattern");
            }

            IsPrepared = true;
        }
    }
}
