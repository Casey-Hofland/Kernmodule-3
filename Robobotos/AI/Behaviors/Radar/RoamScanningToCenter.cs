using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using Robocode.Util;
using System.Drawing;

namespace CaseyDeCoder
{
    public class RoamScanningToCenter : RoamScanning
    {
        public RoamScanningToCenter(AIRobot robot) : base(robot) { }

        public override void Start()
        {
            base.Start();

            robot.RadarColor = robot.ScanColor = Color.Green;

            var centerX = robot.BattleFieldWidth / 2;
            var centerY = robot.BattleFieldHeight / 2;

            // Get the degrees from the robot towards the center of the stage in robocode angles.
            var angle = Math.Atan2(robot.Y - centerY, robot.X - centerX);
            var angleDegrees = Utils.ToDegrees(angle);
            var robotToCenterAngle = ((angleDegrees * -1) + 270) % 360;

            // Compare the previous angle with the radar heading.
            var a = robotToCenterAngle - robot.RadarHeading;
            a += (a > 180) ? -360 : (a < -180) ? 360 : 0;

            direction = (a >= 0) ? Direction.Right : Direction.Left;
        }
    }
}
