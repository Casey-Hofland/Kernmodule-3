using Robocode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public class RoamScanning : NodeBase
    {
        protected Direction turnDirection = Direction.Right;

        public RoamScanning() { }
        public RoamScanning(Direction direction) 
        {
            turnDirection = direction;
        }

        // Turns as fast as possible in the turnDirection.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(blackboard.GetValue<int?>(BB.framesSinceLastScanKey) == 0)
                return TaskStatus.Success;

            try
            {
                var robot = blackboard.GetValue<AdvancedRobot>(BB.robotKey);

                robot.RadarColor = robot.ScanColor = Color.Red;

                robot.SetTurnRadarRightRadians(Rules.RADAR_TURN_RATE_RADIANS * (int)turnDirection);
            }
            catch(NullReferenceException)
            {
                return TaskStatus.Failed;
            }

            return TaskStatus.Running;
        }
    }
}
