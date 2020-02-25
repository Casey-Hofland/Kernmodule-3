using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder
{
    public class Behavior
    {
        public BehaviorStatus status;

        protected AIRobot robot;

        public Behavior(AIRobot robot)
        {
            this.robot = robot;
        }

        private void Switch(Behavior newBehavior)
        {
            End();
            robot.behaviors.Remove(this);
            robot.behaviors.Add(newBehavior);
            newBehavior.Start();
        }

        public virtual void Start()
        {
            status = BehaviorStatus.Running;
        }

        public void Fail(Behavior newBehavior)
        {
            status = BehaviorStatus.Failed;
            Switch(newBehavior);
        }

        public void Succeed(Behavior newBehavior)
        {
            status = BehaviorStatus.Success;
            Switch(newBehavior);
        }

        public virtual void End()
        {

        }

        public virtual void Execute()
        {

        }

        public virtual void OnBattleEnded(BattleEndedEvent evnt)
        {

        }

        public virtual void OnBulletHit(BulletHitEvent evnt)
        {

        }

        public virtual void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {

        }

        public virtual void OnBulletMissed(BulletMissedEvent evnt)
        {

        }

        public virtual void OnCustomEvent(CustomEvent evnt)
        {

        }

        public virtual void OnDeath(DeathEvent evnt)
        {

        }

        public virtual void OnHitByBullet(HitByBulletEvent evnt)
        {

        }

        public virtual void OnHitRobot(HitRobotEvent evnt)
        {

        }

        public virtual void OnHitWall(HitWallEvent evnt)
        {

        }

        public virtual void OnPaint(IGraphics graphics)
        {

        }

        public virtual void OnRobotDeath(RobotDeathEvent evnt)
        {

        }

        public virtual void OnRoundEnded(RoundEndedEvent evnt)
        {

        }

        public virtual void OnScannedRobot(ScannedRobotEvent evnt)
        {

        }

        public virtual void OnSkippedTurn(SkippedTurnEvent evnt)
        {

        }

        public virtual void OnStatus(StatusEvent e)
        {

        }

        public virtual void OnWin(WinEvent evnt)
        {

        }

        public virtual void Tick()
        {

        }
    }
}
