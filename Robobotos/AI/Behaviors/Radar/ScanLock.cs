using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using System.Drawing;
using Robocode.Util;

namespace CaseyDeCoder
{
    public class ScanLock : Behavior
    {
        private int ticksSinceLastScan = 0;
        private const int tickMissMargin = 1;

        protected double lockFactor;

        public ScanLock(AIRobot robot, double lockFactor) : base(robot) 
        {
            this.lockFactor = lockFactor;
        }

        public override void Start()
        {
            base.Start();

            robot.RadarColor = robot.ScanColor = Color.Silver;
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            double absoluteBearing = evnt.BearingRadians + robot.HeadingRadians;
            double radarTurn = absoluteBearing - robot.RadarHeadingRadians;
            robot.SetTurnRadarRightRadians(lockFactor * Utils.NormalRelativeAngle(radarTurn));

            ticksSinceLastScan = 0;
        }

        public override void Tick()
        {
            ++ticksSinceLastScan;
            robot.Scan();

            if(ticksSinceLastScan > tickMissMargin)
            {
                var newBehavior = new RoamScanningToCenter(robot);
                Fail(newBehavior);
                newBehavior.Tick();
            }
        }
    }
}
