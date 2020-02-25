using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder
{
    public class FixedLock : ScanLock
    {
        public FixedLock(AIRobot robot) : base(robot, 2.0) { }
    }
}
