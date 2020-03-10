using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public class Selector : NodeBase
    {
        public Selector(params NodeBase[] nodes) : base(nodes)
        {
        }

        // Execute the first node that's not failing.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            foreach(var node in nodes)
            {
                var taskStatus = node.Tick(blackboard);
                switch(taskStatus)
                {
                    case TaskStatus.Failed:
                        break;
                    case TaskStatus.Running:
                    case TaskStatus.Success:
                        return taskStatus;
                }
            }

            return TaskStatus.Failed;
        }
    }
}
