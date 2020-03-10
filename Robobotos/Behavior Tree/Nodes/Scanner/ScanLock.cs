using Robocode;
using Robocode.Util;
using System;
using System.Drawing;

namespace CaseyDeCoder.BehaviorTree
{
    public class ScanLock : NodeBase
    {
        private const int tickMissMargin = 1;

        protected double lockFactor;

        public ScanLock(double lockFactor)
        {
            this.lockFactor = lockFactor;
        }

        // Lock the scanner on the enemy based on the lockFactor.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            try
            {
                var robot = blackboard.GetValue<AdvancedRobot>(BB.robotKey);
                robot.Scan();   // If this function isn't called inside the Lock, it will not work.

                robot.RadarColor = robot.ScanColor = Color.White;

                if(blackboard.GetValue<int?>(BB.framesSinceLastScanKey) > tickMissMargin)
                    return TaskStatus.Failed;

                // Turn the scanner in the direction where it will pass the scanned robot.
                var lastScannedRobot = blackboard.GetValue<ScannedRobotEvent>(BB.lastScannedEventKey);

                double absoluteBearing = lastScannedRobot.BearingRadians + robot.HeadingRadians;
                double radarTurn = absoluteBearing - robot.RadarHeadingRadians;
                robot.SetTurnRadarRightRadians(lockFactor * Utils.NormalRelativeAngle(radarTurn));
            }
            catch(NullReferenceException)
            {
                return TaskStatus.Failed;
            }

            return TaskStatus.Running;
        }
    }
}
