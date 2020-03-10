using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public class Looper : Sequencer
    {
        protected int loops = 0;
        protected double margin = double.PositiveInfinity;

        public Looper(params NodeBase[] nodes) : base(nodes)
        {
        }

        public Looper(int margin, params NodeBase[] nodes) : this(nodes)
        {
            this.margin = Math.Max(0, margin);  // Margin may never be less than 0.
        }

        // Loops through the nodes as many times as the margin.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            var taskStatus = base.Tick(blackboard);
            if(taskStatus == TaskStatus.Success)
            {
                ++loops;
                return loops > margin ? TaskStatus.Success : TaskStatus.Failed;
            }
            return taskStatus;
        }
    }
}
