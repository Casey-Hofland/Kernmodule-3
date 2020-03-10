using Robocode;
using System.Drawing;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public class RapidFire : NodeBase
    {
        // Shoot as fast as possible wherever the gun is pointing.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.framesSinceLastScanKey, out int framesSinceLastScan)
                || !blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot))
                return TaskStatus.Failed;

            robot.GunColor = robot.BulletColor = Color.Silver;

            if(framesSinceLastScan > 0)
                return TaskStatus.Running;

            robot.SetFireBullet(1);

            if(!blackboard.TryGetValue(BB.lastEnemyPositionKey, out Vector lastEnemyPosition))
                return TaskStatus.Failed;

            return TaskStatus.Running;
        }
    }
}
