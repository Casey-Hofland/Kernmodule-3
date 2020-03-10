using Robocode;
using System.Drawing;

namespace CaseyDeCoder.BehaviorTree
{
    public class TurnGunWithScanner : NodeBase
    {
        // Turn the gun together with the direction the scanner is turning.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(blackboard.GetValue<int?>(BB.framesSinceLastScanKey) == 0)
                return TaskStatus.Success;

            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || !blackboard.TryGetValue(BB.lastRadarTurnDirectionKey, out int direction))
                return TaskStatus.Failed;

            robot.GunColor = robot.BulletColor = Color.Green;

            robot.SetTurnGunRight(Rules.GUN_TURN_RATE * direction);
            return TaskStatus.Running;
        }
    }
}
