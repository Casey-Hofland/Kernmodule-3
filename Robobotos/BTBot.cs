using Robocode;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public class BTBot : AdvancedRobot
    {
        public Blackboard blackboard = new Blackboard();
        public List<NodeBase> behaviorTrees = new List<NodeBase>();

        private RobotStatus status;

        public BTBot()
        {
            blackboard.SetValue(BB.robotKey, this);
            blackboard.SetValue(BB.enemyPositionLogKey, new Vector[7]);
            blackboard.SetValue(BB.bulletsKey, new List<Bullet>());

            // Scanner behavior tree
            behaviorTrees.Add
            (
                new Sequencer(
                    new NoRepeat(new RoamScanningToCenter())
                    , new NoRepeat(new ContinueOnFail(new ThinLock()), 2)
                    , new NoRepeat(new RoamScanningToOppositeDirection())
                    , new NoRepeat(new ContinueOnFail(new NarrowLock()))
                    , new Looper(
                        new RoamScanningToOppositeDirection()
                        , new ContinueOnFail(new FixedLock())
                    )
                )
            );

            // Gun behavior tree
            behaviorTrees.Add
            (
                new Sequencer(
                    new NoRepeat(new TurnGunWithScanner())
                    , new NoRepeat(new TurnGunToEnemy())
                    , new FireAtEnemy()
                )
            );

            // Wheels behavior tree
            behaviorTrees.Add
            (
                new Selector(
                    new Ram()
                    , new AwayFromWall()
                    , new Spin()
                )
            );
        }

        // Override methods and set values in the blackboard if relevant.
        public override void Execute()
        {
        }

        public override void OnBattleEnded(BattleEndedEvent evnt)
        {
        }

        public override void OnBulletHit(BulletHitEvent evnt)
        {
            var bullets = blackboard.GetValue<List<Bullet>>(BB.bulletsKey);
            bullets.Remove(evnt.Bullet);
            blackboard.SetValue(BB.bulletsKey, bullets);
        }

        public override void OnBulletHitBullet(BulletHitBulletEvent evnt)
        {
            var bullets = blackboard.GetValue<List<Bullet>>(BB.bulletsKey);
            bullets.Remove(evnt.Bullet);
            blackboard.SetValue(BB.bulletsKey, bullets);
        }

        public override void OnBulletMissed(BulletMissedEvent evnt)
        {
            var bullets = blackboard.GetValue<List<Bullet>>(BB.bulletsKey);
            bullets.Remove(evnt.Bullet);
            blackboard.SetValue(BB.bulletsKey, bullets);
        }

        public override void OnCustomEvent(CustomEvent evnt)
        {
        }

        public override void OnDeath(DeathEvent evnt)
        {
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
        }

        public override void OnHitRobot(HitRobotEvent evnt)
        {
        }

        public override void OnHitWall(HitWallEvent evnt)
        {
        }

        public override void OnPaint(IGraphics graphics)
        {
        }

        public override void OnRobotDeath(RobotDeathEvent evnt)
        {
        }

        public override void OnRoundEnded(RoundEndedEvent evnt)
        {
        }

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            blackboard.SetValue(BB.lastScannedEventKey, evnt);
            blackboard.SetValue(BB.framesSinceLastScanKey, 0);

            // Log the enemy's position
            double angleToEnemy = evnt.Bearing;
            double angle = (Math.PI / 180) * ((status.Heading + angleToEnemy) % 360);

            Vector lastEnemyPosition = new Vector
            {
                X = status.X + Math.Sin(angle) * evnt.Distance
                , Y = status.Y + Math.Cos(angle) * evnt.Distance
            };

            blackboard.SetValue(BB.lastEnemyPositionKey, lastEnemyPosition);

            // Update the enemy position log
            var enemyPositionLog = blackboard.GetValue<Vector[]>(BB.enemyPositionLogKey);
            var logIndex = blackboard.GetValue<int>(BB.enemyPositionLogIndexKey);

            enemyPositionLog[logIndex] = lastEnemyPosition;

            ++logIndex;
            logIndex %= enemyPositionLog.Length;

            blackboard.SetValue(BB.enemyPositionLogKey, enemyPositionLog);
            blackboard.SetValue(BB.enemyPositionLogIndexKey, logIndex);
        }

        public override void OnSkippedTurn(SkippedTurnEvent evnt)
        {
        }

        public override void OnStatus(StatusEvent evnt)
        {
            status = evnt.Status;
        }

        public override void OnWin(WinEvent evnt)
        {
        }

        public override void Run()
        {
            // These actions should always be the first 2 called in runso your robot's basegun and radar can rotate independently. Essential for radar locks and accurate targeting.
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;

            // Initialize the behavior trees
            for(int i = 0; i < behaviorTrees.Count; ++i)
                behaviorTrees[i].Initialize(blackboard);

            while(true)
            {
                // Run the behavior trees
                for(int i = 0; i < behaviorTrees.Count; ++i)
                {
                    var taskStatus = behaviorTrees[i].Tick(blackboard);
                    if(taskStatus == TaskStatus.Success)
                        behaviorTrees.RemoveAt(i);
                }

                blackboard.SetValue(BB.lastRadarTurnDirectionKey, (RadarTurnRemainingRadians >= 0 ? Direction.Right : Direction.Left));
                blackboard.SetValue(BB.framesSinceLastScanKey, blackboard.GetValue<int>(BB.framesSinceLastScanKey) + 1);

                // Call these functions to complete one tick.
                Execute();
                DoNothing();
            }
        }
    }
}
