using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;

namespace CaseyDeCoder
{
    public class AIRobot : AdvancedRobot
    {
        public List<Behavior> behaviors;

        public AIRobot()
        {
            behaviors = new List<Behavior>()
            {
                new RoamScanningToCenter(this)
            };
        }

        public override void Execute()
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.Execute();
            }
        }

        public override void OnBattleEnded(BattleEndedEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnBattleEnded(evnt);
            }
        }

        public override void OnBulletHit(BulletHitEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnBulletHit(evnt);
            }
        }

        public override void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnBulletHitBullet(evnt);
            }
        }

        public override void OnBulletMissed(BulletMissedEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnBulletMissed(evnt);
            }
        }

        public override void OnCustomEvent(CustomEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnCustomEvent(evnt);
            }
        }

        public override void OnDeath(DeathEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnDeath(evnt);
            }
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnHitByBullet(evnt);
            }
        }

        public override void OnHitRobot(HitRobotEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnHitRobot(evnt);
            }
        }

        public override void OnHitWall(HitWallEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnHitWall(evnt);
            }
        }

        public override void OnPaint(IGraphics graphics)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnPaint(graphics);
            }
        }

        public override void OnRobotDeath(RobotDeathEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnRobotDeath(evnt);
            }
        }

        public override void OnRoundEnded(RoundEndedEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnRoundEnded(evnt);
            }
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnScannedRobot(evnt);
            }
        }

        public override void OnSkippedTurn(SkippedTurnEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnSkippedTurn(evnt);
            }
        }

        public override void OnStatus(StatusEvent e)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnStatus(e);
            }
        }

        public override void OnWin(WinEvent evnt)
        {
            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.OnWin(evnt);
            }
        }

        public override void Run()
        {
            // These actions should always be the first 2 called in runso your robot's basegun and radar can rotate independently. Essential for radar locks and accurate targeting.
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;

            for(int i = 0; i < behaviors.Count; ++i)
            {
                var behavior = behaviors[i];
                behavior.Start();
            }

            while(true)
            {
                for(int i = 0; i < behaviors.Count; ++i)
                {
                    var behavior = behaviors[i];
                    behavior.Tick();
                }

                Execute();
                DoNothing();
            }
        }
    }
}
