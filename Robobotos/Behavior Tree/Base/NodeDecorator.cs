using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder.BehaviorTree
{
    public abstract class NodeDecorator : NodeBase
    {
        protected NodeBase node;

        public NodeDecorator(NodeBase node)
        {
            this.node = node;
        }

        public override void Initialize(Blackboard blackboard)
        {
            node.Initialize(blackboard);
        }

        public override TaskStatus Tick(Blackboard blackboard)
        {
            return node.Tick(blackboard);
        }
    }
}
