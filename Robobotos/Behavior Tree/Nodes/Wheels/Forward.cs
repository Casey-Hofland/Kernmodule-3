using Robocode;
using System.Drawing;

namespace CaseyDeCoder.BehaviorTree
{
    public class Forward : MoveNodeBase
    {
        // Moves the robot forward.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot))
                return TaskStatus.Failed;

            robot.BodyColor = Color.Gray;

            robot.SetAhead(Rules.MAX_VELOCITY);

            return TaskStatus.Running;
        }
    }
}
