using Robocode;
using System.Drawing;

namespace CaseyDeCoder.BehaviorTree
{
    public class Spin : MoveNodeBase
    {
        // Moves and turns.
        public override TaskStatus Tick(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot))
                return TaskStatus.Failed;

            robot.BodyColor = Color.Blue;

            robot.SetTurnRight(Rules.MAX_TURN_RATE);
            robot.SetAhead(Rules.MAX_VELOCITY);

            return TaskStatus.Running;
        }
    }
}
