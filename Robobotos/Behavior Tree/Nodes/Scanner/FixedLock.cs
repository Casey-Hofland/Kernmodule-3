using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder.BehaviorTree
{
    public class FixedLock : ScanLock
    {
        // A fixed lock remains wide and will never fail but may not scan the enemy every turn.
        public FixedLock() : base(2.0) { }

        public override TaskStatus Tick(Blackboard blackboard)
        {
            var tick = base.Tick(blackboard);

            if(blackboard.TryGetValue<AdvancedRobot>(BB.robotKey, out AdvancedRobot robot))
                robot.RadarColor = robot.ScanColor = Color.Tan;

            return tick;
        }
    }
}
