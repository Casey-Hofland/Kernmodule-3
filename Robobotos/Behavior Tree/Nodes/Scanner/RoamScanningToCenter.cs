using CaseyDeCoder.Utilities;
using Robocode;
using System;
using System.Drawing;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public class RoamScanningToCenter : RoamScanning
    {
        public override void Initialize(Blackboard blackboard)
        {
            try
            {
                // Figure out the direction to turn to scan the center the quickest. This ensures that the biggest area from this position is scanned first.
                var robot = blackboard.GetValue<AdvancedRobot>(BB.robotKey);

                var center = new Vector
                {
                    X = robot.BattleFieldWidth / 2
                    , Y = robot.BattleFieldHeight / 2
                };

                // Get the degrees from the robot towards the center of the stage in robocode angles.
                var robotToCenterAngle = Utility.Angle(robot.X, robot.Y, center.X, center.Y);

                // Compare the previous angle with the radar heading.
                var radarToCenterAngle = robotToCenterAngle - robot.RadarHeading;
                radarToCenterAngle += (radarToCenterAngle > 180) ? -360 : (radarToCenterAngle < -180) ? 360 : 0;

                turnDirection = ((radarToCenterAngle >= 0) ? Direction.Right : Direction.Left);
            }
            catch(NullReferenceException) { }
        }

        public override TaskStatus Tick(Blackboard blackboard)
        {
            var tick = base.Tick(blackboard);

            if(blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot))
                robot.RadarColor = robot.ScanColor = Color.Green;

            return tick;
        }
    }
}
