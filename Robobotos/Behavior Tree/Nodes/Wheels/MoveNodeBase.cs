using Robocode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.BehaviorTree
{
    public abstract class MoveNodeBase : NodeBase
    {
        protected const double wallMargin = 75.0;

        protected static bool TooCloseToWall(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot))
                return false;

            return robot.X < wallMargin || robot.X > robot.BattleFieldWidth - wallMargin
                || robot.Y < wallMargin || robot.Y > robot.BattleFieldHeight - wallMargin;
        }
    }
}
