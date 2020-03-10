using Robocode;
using System;
using System.Drawing;

namespace CaseyDeCoder.BehaviorTree
{
    public class FireAtEnemy : GunNodeBase
    {
        private double headingMargin = 2.0;

        public FireAtEnemy() : base() { }

        public FireAtEnemy(double headingMargin) : this()
        {
            this.headingMargin = headingMargin;
        }

        // Turn the gun towards the enemy and fire when appropriate.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || !blackboard.TryGetValue(BB.framesSinceLastScanKey, out int framesSinceLastScan))
                return TaskStatus.Failed;

            robot.GunColor = robot.BulletColor = Color.Gold;

            var gunToEnemyAngle = Math.Min(GunToEnemyAngle(blackboard), Rules.GUN_TURN_RATE);

            robot.SetTurnGunRight(gunToEnemyAngle);

            if(framesSinceLastScan == 0 && Math.Abs(robot.GunHeading - robot.RadarHeading) < headingMargin)
                Fire(blackboard);

            return TaskStatus.Running;
        }
    }
}
