using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder.BehaviorTree
{
    public class NarrowLock : ScanLock
    {
        // A narrow lock starts wide and thins out to become a (almost identical) thin lock
        public NarrowLock() : base(1.9) { }

        public override TaskStatus Tick(Blackboard blackboard)
        {
            var tick = base.Tick(blackboard);

            if(blackboard.TryGetValue<AdvancedRobot>(BB.robotKey, out AdvancedRobot robot))
                robot.RadarColor = robot.ScanColor = Color.Silver;

            return tick;
        }
    }
}
