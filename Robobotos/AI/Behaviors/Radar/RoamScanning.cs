using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Robocode;

namespace CaseyDeCoder
{
    public class RoamScanning : Behavior
    {
        protected Direction direction;

        public RoamScanning(AIRobot robot) : this(robot, Direction.Right) { }
        public RoamScanning(AIRobot robot, Direction direction) : base(robot)
        {
            this.direction = direction;
        }

        public override void Start()
        {
            base.Start();

            robot.RadarColor = robot.ScanColor = Color.Red;
        }

        public override void Tick()
        {
            robot.SetTurnRadarRight(Rules.RADAR_TURN_RATE * (int)direction);
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            var newBehavior = new ThinLock(robot);
            Succeed(newBehavior);
            newBehavior.OnScannedRobot(evnt);
        }
    }
}
