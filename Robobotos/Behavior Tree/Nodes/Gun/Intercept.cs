using Robocode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public class Intercept : GunNodeBase
    {
        private double headingMargin = 2.0;

        public Intercept() : base() { }

        public Intercept(double headingMargin) : this()
        {
            this.headingMargin = headingMargin;
        }

        // Predict where the enemy is going to move and shoot there.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || !blackboard.TryGetValue(BB.framesSinceLastScanKey, out int framesSinceLastScan))
                return TaskStatus.Failed;

            //var gunToEnemyAngle = GunToEnemyAngle(blackboard);
            //robot.SetTurnGunRight(gunToEnemyAngle);

            // Predictive Shooting


            if(framesSinceLastScan == 0 && Math.Abs(robot.GunHeading - robot.RadarHeading) < headingMargin)
                Fire(blackboard);

            return TaskStatus.Running;
        }
    }
}
