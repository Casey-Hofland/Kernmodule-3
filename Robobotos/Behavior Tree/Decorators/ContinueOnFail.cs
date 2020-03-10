using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder.BehaviorTree
{
    public class ContinueOnFail : NodeDecorator
    {
        public ContinueOnFail(NodeBase node) : base(node)
        {
        }

        // When a node returns a fail, return a success and the other way around. This can trick CollectionNodes into passing or stopping at certain nodes that would otherwise be incorrectly execuded.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            var taskStatus = base.Tick(blackboard);
            switch(taskStatus)
            {
                case TaskStatus.Failed:
                    return TaskStatus.Success;
                case TaskStatus.Success:
                    return TaskStatus.Failed;
                default:
                    return taskStatus;
            }
        }
    }
}
