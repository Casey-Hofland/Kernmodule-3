using CaseyDeCoder.Utilities;
using Robocode;
using System;
using System.Drawing;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public class Ram : MoveNodeBase
    {
        protected double energyDifference = 10.0;
        protected double moveAngleMargin = 45.0;

        public Ram() : base() { }

        public Ram(double energyDifference, double moveAngleMargin) : this()
        {
            this.energyDifference = energyDifference;
            this.moveAngleMargin = moveAngleMargin;
        }

        // Move towards the other robot and try to ram him.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || !blackboard.TryGetValue(BB.lastScannedEventKey, out ScannedRobotEvent evnt)
                || robot.Energy - evnt.Energy < energyDifference
                || !blackboard.TryGetValue(BB.lastEnemyPositionKey, out Vector lastEnemyPosition))
                return TaskStatus.Failed;

            robot.BodyColor = Color.Gold;

            var robotToEnemyAngle = Utility.Angle(robot.X, robot.Y, lastEnemyPosition.X, lastEnemyPosition.Y);

            var headingToEnemyAngle = robotToEnemyAngle - robot.Heading;
            var turnRate = headingToEnemyAngle + (headingToEnemyAngle > 180 ? -360 : (headingToEnemyAngle < -180 ? 360 : 0));
            turnRate = Math.Min(turnRate, Rules.MAX_TURN_RATE);

            robot.SetTurnRight(turnRate);
            if(Math.Abs(headingToEnemyAngle) < moveAngleMargin)
                robot.SetAhead(Rules.MAX_VELOCITY);

            return TaskStatus.Running;
        }
    }
}
