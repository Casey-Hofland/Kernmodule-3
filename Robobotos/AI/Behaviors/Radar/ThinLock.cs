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
    public class ThinLock : ScanLock
    {
        public ThinLock(AIRobot robot) : base(robot, 1.0) { }
    }
}
