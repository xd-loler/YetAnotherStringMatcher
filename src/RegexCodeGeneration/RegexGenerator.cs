using System;
using System.Text;
using YetAnotherStringMatcher;
using RegexCodeGeneration.Nodes;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RegexCodeGeneration
{
    public class RegexGenerator
    {
        private StringBuilder StringBuilder { get; set; } = new StringBuilder();

        public List<IOperation> Requirements { get; }

        public IRequirementConverter Converter { get; }

        public RegexGenerator(List<IOperation> Requirements)
        {
            this.Requirements = Requirements;
            Converter = new RequirementConverter();
        }

        public RegexGenerator(List<IOperation> Requirements, IRequirementConverter converter)
        {
            this.Requirements = Requirements;
            Converter = converter;
        }

        public CodeGenResult Emit()
        {
            try
            {
                var node = Converter.Convert(Requirements);
                InternalEmit(node);
            }
            catch (Exception ex)
            {
                return CodeGenResult.ErrorResult(ex.Message);
            }

            return CodeGenResult.SuccessResult(StringBuilder.ToString());
        }

        private void InternalEmit(Node node)
        {
            // yea, I'm not a fan of visitor...

            if (node is RootNode rn)
            {

            }
            else if (node is ThenAnythingNode tan)
            {
                IgnoreCase(tan.IgnoreCase);

                StringBuilder.Append(".");

                IsOptional(tan.IsOptional);
            }
            else if (node is NoMoreNode nmn)
            {
                StringBuilder.Append("$");
            }
            else if (node is AnyOfNode aon)
            {
                // Should we be worried if Items list is empty?

                IgnoreCase(aon.IgnoreCase);

                StringBuilder.Append("(");

                for (int i = 0; i < aon.Items.Count; i++)
                {
                    var current = Escape(aon.Items[i]);
                    StringBuilder.Append(current);

                    if (i != aon.Items.Count - 1)
                        StringBuilder.Append("|");
                }

                StringBuilder.Append(")");

                IsOptional(aon.IsOptional);
            }
            else if (node is ThenNode tn)
            {
                IgnoreCase(tn.IgnoreCase);

                StringBuilder.Append("(");
                StringBuilder.Append(Escape(tn.Item));
                StringBuilder.Append(")");

                IsOptional(tn.IsOptional);
            }
            else if (node is SymbolsWithLenghtBetweenNode swlbn)
            {
                if (swlbn.Items.Any())
                {
                    IgnoreCase(swlbn.IgnoreCase);

                    StringBuilder.Append("[");

                    for (int i = 0; i < swlbn.Items.Count; i++)
                    {
                        var current = Escape(swlbn.Items[i]);
                        StringBuilder.Append(current);
                    }

                    StringBuilder.Append("]");

                    StringBuilder.Append($"{{{swlbn.Length}}}");

                    IsOptional(swlbn.IsOptional);
                }
            }
            else if (node is DigitsWithLenghtBetweenNode dwlbn)
            {
                StringBuilder.Append(@"\d");
                StringBuilder.Append($"{{{dwlbn.MinimumLength},{dwlbn.MaximumLength}}}");

                IsOptional(dwlbn.IsOptional);
            }
            else if (node is DigitsOfLengthNode dolbn)
            {
                StringBuilder.Append(@"\d");
                StringBuilder.Append($"{{{dolbn.Length}}}");

                IsOptional(dolbn.IsOptional);
            }
            else
            {
                throw new NotImplementedException($"Node of type: {node.GetType()} is not implemented yet");
            }

            EmitSubNodes(node);
        }

        private string Escape(string input)
        {
            return Regex.Escape(input);
        }

        private void IsOptional(bool isOptional)
        {
            if (isOptional)
                StringBuilder.Append("?");
        }

        private void IgnoreCase(bool ignoreCase)
        {
            if (ignoreCase)
                StringBuilder.Append("(?i)");
        }

        private void EmitSubNodes(Node node)
        {
            foreach (var sub_node in node.Children)
            {
                InternalEmit(sub_node);
            }
        }
    }
}
