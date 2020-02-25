using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder
{
    public class StupidBot : AdvancedRobot
    {
        public override void Run()
        {
            while(true)
            {
                Ahead(100);
                Back(100);

                TurnLeft(360 * 3);
            }
        }
    }
}
