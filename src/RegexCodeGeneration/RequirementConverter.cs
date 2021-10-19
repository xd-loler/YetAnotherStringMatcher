using System;
using System.Linq;
using YetAnotherStringMatcher;
using RegexCodeGeneration.Nodes;
using System.Collections.Generic;
using YetAnotherStringMatcher.Requirements;

namespace RegexCodeGeneration
{
    public class RequirementConverter : IRequirementConverter
    {
        public Node Convert(List<IOperation> requirements)
        {
            var root = new RootNode();

            for (int i = 0; i < requirements.Count; i++)
            {
                var current = requirements[i];

                if (current is AnythingRequirement ar)
                {
                    root.AddChild(new ThenAnythingNode(ar.Options));
                }
                else if (current is EndRequirement er)
                {
                    root.AddChild(new NoMoreNode());
                }
                else if (current is AnyOfRequirement aor)
                {
                    root.AddChild(new AnyOfNode(aor.Items.ToList(), aor.Options));
                }
                else if (current is ThenRequirement tr)
                {
                    root.AddChild(new ThenNode(tr.Item, tr.Options));
                }
                else if (current is SymbolsOfLengthRequirement swlbr)
                {
                    root.AddChild(new SymbolsWithLenghtBetweenNode(swlbr.Symbols, swlbr.Length, swlbr.Options));
                }
                else if (current is DigitsOfLenghtRequirement dolr)
                {
                    root.AddChild(new DigitsOfLengthNode(dolr.Length, dolr.Options));
                }
                else if (current is DigitsWithLenghtBetweenRequirement dwlbr)
                {
                    root.AddChild(new DigitsWithLenghtBetweenNode(dwlbr.MinimumLength, dwlbr.MaximumLength, dwlbr.Options));
                }
                else
                {
                    throw new NotSupportedException($"Requirement of type: {current.GetType()} is not supported (yet?)");
                }
            }

            return root;
        }
    }
}