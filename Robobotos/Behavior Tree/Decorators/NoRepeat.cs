using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public class NoRepeat : NodeDecorator
    {
        protected int succeeds = 0;
        protected int triesMargin = 0;

        public NoRepeat(NodeBase node) : base(node)
        {
        }

        public NoRepeat(NodeBase node, int tries) : this(node)
        {
            triesMargin = Math.Max(0, tries);
        }

        // When this node has repeated more than the triesMargin, don't repeat it anymore.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(succeeds > triesMargin)
                return TaskStatus.Success;

            var taskStatus = base.Tick(blackboard);
            if(taskStatus == TaskStatus.Success)
                ++succeeds;
            return taskStatus;
        }
    }
}
