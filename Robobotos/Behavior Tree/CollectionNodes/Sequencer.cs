using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public class Sequencer : NodeBase
    {
        public Sequencer(params NodeBase[] nodes) : base(nodes)
        {
        }

        // Select the first node that is not executing successfuly.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            foreach(var node in nodes)
            {
                var taskStatus = node.Tick(blackboard);
                switch(taskStatus)
                {
                    case TaskStatus.Failed:
                    case TaskStatus.Running:
                        return taskStatus;
                    case TaskStatus.Success:
                        break;
                }
            }

            return TaskStatus.Success;
        }
    }
}
