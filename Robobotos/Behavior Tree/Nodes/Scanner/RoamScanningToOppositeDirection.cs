using Robocode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    // Scan towards the opposite stored direction (doesn't work).
    public class RoamScanningToOppositeDirection : RoamScanning
    {
        public override void Initialize(Blackboard blackboard)
        {
            if(blackboard.TryGetValue<Direction>(BB.lastRadarTurnDirectionKey, out Direction direction))
                turnDirection = (Direction)((int)direction * -1);
        }

        public override TaskStatus Tick(Blackboard blackboard)
        {
            var tick = base.Tick(blackboard);

            if(blackboard.TryGetValue<AdvancedRobot>(BB.robotKey, out AdvancedRobot robot))
                robot.RadarColor = robot.ScanColor = Color.Blue;

            return tick;
        }
    }
}
