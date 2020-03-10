using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaseyDeCoder.BehaviorTree
{
    public abstract class NodeBase
    {
        protected NodeBase[] nodes;

        public NodeBase(params NodeBase[] nodes)
        {
            this.nodes = nodes;
        }

        public virtual void Initialize(Blackboard blackboard) 
        {
            foreach(var node in nodes)
                node.Initialize(blackboard);
        }

        public abstract TaskStatus Tick(Blackboard blackboard);
    }
}
