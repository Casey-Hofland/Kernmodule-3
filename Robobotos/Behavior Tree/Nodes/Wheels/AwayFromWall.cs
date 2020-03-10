using CaseyDeCoder.Utilities;
using Robocode;
using System;
using System.Drawing;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public class AwayFromWall : MoveNodeBase
    {
        protected double moveAngleMargin = 45.0;
        protected double distanceFromCenterMargin = 150.0;
        private bool movingAway = false;

        public AwayFromWall() : base() { }

        public AwayFromWall(double moveAngleMargin) : this()
        {
            this.moveAngleMargin = moveAngleMargin;
        }

        public override void Initialize(Blackboard blackboard)
        {
            movingAway = false;
        }

        // When too close to the wall, move towards the center until the robot is far enough from the wall.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if((!movingAway && !TooCloseToWall(blackboard))
                || !blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot))
                return TaskStatus.Failed;

            robot.BodyColor = Color.White;

            movingAway = true;

            var center = new Vector
            {
                X = robot.BattleFieldWidth / 2
                , Y = robot.BattleFieldHeight / 2
            };

            // Get the degrees from the robot towards the center of the stage in robocode angles.
            var robotToCenterAngle = Utility.Angle(robot.X, robot.Y, center.X, center.Y);

            // Figure out the turn rate for the robot.
            var headingToCenterAngle = robotToCenterAngle - robot.Heading;
            var turnRate = headingToCenterAngle + (headingToCenterAngle > 180 ? -360 : (headingToCenterAngle < -180 ? 360 : 0));
            turnRate = Math.Min(turnRate, Rules.MAX_TURN_RATE);

            robot.SetTurnRight(turnRate);

            // If the remaining turn angle is less than the moveAngle, start moving.
            if(Math.Abs(headingToCenterAngle) < moveAngleMargin)
                robot.SetAhead(Rules.MAX_VELOCITY);

            // Check if the robot is close enough to the center to stop moving away from the wall.
            var robotToCenterDistanceSquared = Utility.DistanceSquared(robot.X, robot.Y, center.X, center.Y);
            if(robotToCenterDistanceSquared < Math.Pow(distanceFromCenterMargin, 2))
            {
                movingAway = false;
            }

            return TaskStatus.Running;
        }
    }
}
