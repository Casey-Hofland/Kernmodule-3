using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder.BehaviorTree
{
    public class ThinLock : ScanLock
    {
        // A thin lock focuses using as thin a line as possible but may fail on a skipped turn.
        public ThinLock() : base(1.0) { }

        public override TaskStatus Tick(Blackboard blackboard)
        {
            var tick = base.Tick(blackboard);

            if(blackboard.TryGetValue<AdvancedRobot>(BB.robotKey, out AdvancedRobot robot))
                robot.RadarColor = robot.ScanColor = Color.Gold;

            return tick;
        }
    }
}
