using Robocode;
using System;
using System.Drawing;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public class TurnGunToEnemy : GunNodeBase
    {
        private double angleMargin = Rules.GUN_TURN_RATE / 2 + double.Epsilon;

        public TurnGunToEnemy() : base() { }

        public TurnGunToEnemy(double angleMargin) : this()
        {
            this.angleMargin = angleMargin;
        }

        // Turn the gun towards the enemy as fast as possible without shooting.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || !blackboard.TryGetValue(BB.lastEnemyPositionKey, out Vector lastEnemyPosition)
                || blackboard.GetValue<int?>(BB.framesSinceLastScanKey) == null)
                return TaskStatus.Failed;

            robot.GunColor = robot.BulletColor = Color.Red;

            // Return success if the angle left to turn is less than the angleMargin
            var gunToEnemyAngle = GunToEnemyAngle(blackboard);

            if(Math.Abs(gunToEnemyAngle) < angleMargin)
            {
                robot.SetTurnGunRight(gunToEnemyAngle);
                return TaskStatus.Success;
            }

            var gunTurnDirection = ((gunToEnemyAngle >= 0) ? Direction.Right : Direction.Left);
            robot.SetTurnGunRight(Rules.GUN_TURN_RATE * (int)gunTurnDirection);
            return TaskStatus.Running;
        }
    }
}
