using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder
{
    public class NarrowLock : ScanLock
    {
        public NarrowLock(AIRobot robot) : base(robot, 1.9) { }
    }
}
