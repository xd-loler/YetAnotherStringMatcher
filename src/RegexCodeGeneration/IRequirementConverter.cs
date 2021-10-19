using YetAnotherStringMatcher;
using System.Collections.Generic;
using RegexCodeGeneration.Nodes;

namespace RegexCodeGeneration
{
    public interface IRequirementConverter
    {
        Node Convert(List<IOperation> requirements);
    }
}