using System;
using System.Collections.Generic;

namespace RegexCodeGeneration.Nodes
{
    public abstract class Node
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public List<Node> Children { get; set; } = new List<Node>();

        public Node AddChild(Node n)
        {
            this.Children.Add(n);

            return this;
        }

        public Node AddChildren(IEnumerable<Node> n)
        {
            this.Children.AddRange(n);

            return this;
        }

        public abstract override string ToString();
    }
}
