#define OPTIMAL_SCAN

using Robocode;
using Robocode.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CaseyDeCoder
{
    public enum RadarLockType
    {
        Thin
        , Narrow
        , Fixed
    }

    public class MissionXOld : AdvancedRobot
    {
        #region WaveSurfing variables
        public const int bins = 47;
        public static double oppEnergy = 100.0;

        public static double[] surfStats = new double[bins];
        public PointF location;
        public PointF enemyLocation;

        public List<EnemyWave> enemyWaves = new List<EnemyWave>();
        public List<(int direction, double absoluteBearing)> surfings = new List<(int direction, double absoluteBearing)>();
        #endregion

        private const float wallDistanceX = 18, wallDistanceY = 18;
        private RectangleF _fieldRect = default;
        private RectangleF fieldRect
        {
            get
            {
                if(_fieldRect != default 
                    || (wallDistanceX * 2 > BattleFieldWidth || wallDistanceY * 2 > BattleFieldHeight))
                    return _fieldRect;

                _fieldRect = new RectangleF(wallDistanceX, wallDistanceY, (float)BattleFieldWidth - wallDistanceX * 2, (float)BattleFieldHeight - wallDistanceY * 2);
                return _fieldRect;
            }
        }
        private const double wallStick = 160.0;

        private RadarLockType radarLockType = RadarLockType.Thin;

        public override void Run()
        {
            SetColors(Color.IndianRed, Color.Gold, Color.Silver, Color.Silver, Color.Silver);

            // These actions should always be the first 2 called in run, so your robot's base, gun and radar can rotate independently. Essential for radar locks and accurate targeting.
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;

#if OPTIMAL_SCAN
            // The direction to turn towards the center the quickest. 1 means Right, -1 means left.
            int QuickestTurnDirection()
            {
                var centerX = BattleFieldWidth / 2;
                var centerY = BattleFieldHeight / 2;

                // Get the degrees from the robot towards the center of the stage in robocode angles.
                var angle = Math.Atan2(Y - centerY, X - centerX);
                var angleDegrees = Utils.ToDegrees(angle);
                var robotToCenterAngle = ((angleDegrees * -1) + 270) % 360;

                // Compare the previous angle with the radar heading.
                var a = robotToCenterAngle - RadarHeading;
                a += (a > 180) ? -360 : (a < -180) ? 360 : 0;

                return a == 0 ? 1 : Math.Sign(a);
            }

            TurnRadarRight(double.PositiveInfinity * QuickestTurnDirection());
#else
            TurnRadarRight(double.PositiveInfinity);
#endif

            do
            {
                if(radarLockType == RadarLockType.Thin)
                    Scan();


            } while(true);
        }

        /*
        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            double slideFactor = 0.0;
            switch(radarLockType)
            {
                case RadarLockType.Thin:
                    slideFactor = 1.0;
                    break;
                case RadarLockType.Narrow:
                    slideFactor = 1.9;
                    break;
                case RadarLockType.Fixed:
                    slideFactor = 2.0;
                    break;
            }

            double radarTurn = Heading + evnt.Bearing - RadarHeading;
            TurnRadarRight(slideFactor * Utils.NormalRelativeAngleDegrees(radarTurn));
        }
        */

        public override void OnScannedRobot(ScannedRobotEvent evnt)
        {
            location = new PointF((float)X, (float)Y);

            double lateralVelocity = Velocity * Math.Sin(evnt.BearingRadians);
            double absoluteBearing = evnt.BearingRadians + HeadingRadians;

            double radarTurn = absoluteBearing - RadarHeadingRadians;

            TurnRadarRightRadians(2 * Utils.NormalRelativeAngle(radarTurn));

            surfings.Insert(0, ((lateralVelocity >= 0 ? 1 : -1), absoluteBearing + Math.PI));

            double bulletPower = oppEnergy - evnt.Energy;
            if(bulletPower < 3.01 && bulletPower > 0.09 && surfings.Count > 2)
            {
                var enemyWave = new EnemyWave
                {
                    fireTime = Time - 1
                    , bulletVelocity = BulletVelocity(bulletPower)
                    , distanceTraveled = BulletVelocity(bulletPower)
                    , direction = surfings[2].direction
                    , directionAngle = surfings[2].absoluteBearing
                    , fireLocation = enemyLocation
                };

                enemyWaves.Add(enemyWave);
            }

            oppEnergy = evnt.Energy;

            // update after EnemyWave detection, because that needs the previous enemy location as the source of the wave.
            enemyLocation = Project(location, absoluteBearing, evnt.Distance);

            UpdateWaves();
            DoSurfing();

            // gun code...
        }

        public double DistanceSquared(PointF point1, PointF point2) => Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2);
        public double Distance(PointF point1, PointF point2) => Math.Sqrt(DistanceSquared(point1, point2));
        public double DistanceFromBulletSquared(EnemyWave enemyWave) => DistanceSquared(location, enemyWave.fireLocation);
        public double DistanceFromBullet(EnemyWave enemyWave) => Math.Sqrt(DistanceFromBulletSquared(enemyWave));

        public void UpdateWaves()
        {
            for(int i = 0; i < enemyWaves.Count; ++i)
            {
                var enemyWave = enemyWaves[i];

                enemyWave.distanceTraveled = (Time - enemyWave.fireTime) * enemyWave.bulletVelocity;
                if(Math.Pow(enemyWave.distanceTraveled, 2) > DistanceFromBulletSquared(enemyWave) + Math.Pow(50, 2))
                {
                    enemyWaves.RemoveAt(i);
                    --i;
                }
            }
        }

        public EnemyWave ClosestSurfableWave()
        {
            double closestDistanceSquared = double.PositiveInfinity;
            EnemyWave surfWave = null;

            for(int i = 0; i < enemyWaves.Count; ++i)
            {
                var enemyWave = enemyWaves[i];
                double distanceSquared = DistanceFromBulletSquared(enemyWave) - Math.Pow(enemyWave.distanceTraveled, 2);

                if(distanceSquared > Math.Pow(enemyWave.bulletVelocity, 2) 
                    && distanceSquared < closestDistanceSquared)
                {
                    surfWave = enemyWave;
                    closestDistanceSquared = distanceSquared;
                }
            }

            return surfWave;
        }

        // Given the EnemyWave that the bullet was on, and the point where we were hit, calculate the index into our stat array for that factor.
        public static int FactorIndex(EnemyWave enemyWave, PointF target)
        {
            double offsetAngle = AbsoluteBearing(enemyWave.fireLocation, target) - enemyWave.directionAngle;
            double factor = Utils.NormalRelativeAngle(offsetAngle) / MaxEscapeAngle(enemyWave.bulletVelocity) * enemyWave.direction;

            var maxFactor = bins - 1;
            return (int)Clamp(factor * (maxFactor / 2) + (maxFactor / 2), 0, maxFactor);
        }

        // Given the EnemyWave that the bullet was on, and the point where we were hit, update our stat array to reflect the danger in that area.
        public static void LogHit(EnemyWave enemyWave, PointF target)
        {
            int index = FactorIndex(enemyWave, target);

            for(int i = 0; i < bins; ++i)
            {
                // for the spot bin that we were hit on, add 1;
                // for the bins next to it, add 1 / 2;
                // the next one, add 1 / 5; and so on...
                surfStats[i] += 1.0 / (Math.Pow(index - i, 2) + 1);
            }
        }

        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            PointF hitBulletLocation = new PointF((float)evnt.Bullet.X, (float)evnt.Bullet.Y);
            EnemyWave hitWave = null;

            // look through the EnemyWaves, and find one that could've hit us.
            for(int i = 0; i < enemyWaves.Count; ++i)
            {
                EnemyWave enemyWave = enemyWaves[i];

                if(Math.Abs(enemyWave.distanceTraveled - DistanceFromBullet(enemyWave)) < 50
                    && Math.Abs(BulletVelocity(evnt.Bullet.Power) - enemyWave.bulletVelocity) < 0.001) 
                {
                    hitWave = enemyWave;
                    break;
                }
            }

            if(hitWave != null)
            {
                LogHit(hitWave, hitBulletLocation);

                // We can remove this wave now, of course.
                enemyWaves.RemoveAt(enemyWaves.LastIndexOf(hitWave));
            }
        }

        public PointF PredictPosition(EnemyWave surfWave, int direction)
        {
            PointF predictedPosition = location;
            double predictedVelocity = Velocity;
            double predictedHeading = HeadingRadians;
            double maxTurning, moveAngle, moveDir;

            int counter = 0;
            bool intercepted = false;

            do
            {
                moveAngle =
                    WallSmoothing(predictedPosition, AbsoluteBearing(surfWave.fireLocation,
                    predictedPosition) + (direction * (Math.PI / 2)), direction)
                    - predictedHeading;
                moveDir = 1;

                if(Math.Cos(moveAngle) < 0)
                {
                    moveAngle += Math.PI;
                    moveDir = -1;
                }

                moveAngle = Utils.NormalRelativeAngle(moveAngle);

                // maxTurning is built in like this, you can't turn more then this in one tick
                maxTurning = Math.PI / 720d * (40d - 3d * Math.Abs(predictedVelocity));
                predictedHeading = Utils.NormalRelativeAngle(predictedHeading
                    + Clamp(-maxTurning, moveAngle, maxTurning));

                // this one is nice ;). if predictedVelocity and moveDir have different signs you want to breack down, otherwise you want to accelerate (look at the factor "2")
                predictedVelocity += (predictedVelocity * moveDir < 0 ? 2 * moveDir : moveDir);
                predictedVelocity = Clamp(-8, predictedVelocity, 8);

                // calculate the new predicted position
                predictedPosition = Project(predictedPosition, predictedHeading, predictedVelocity);

                ++counter;
                intercepted |= Distance(predictedPosition, surfWave.fireLocation) < surfWave.distanceTraveled + (counter * surfWave.bulletVelocity) + surfWave.bulletVelocity;
            } while(!intercepted && counter < 500);

            return predictedPosition;
        }

        public double CheckDanger(EnemyWave surfWave, int direction)
        {
            int index = FactorIndex(surfWave, PredictPosition(surfWave, direction));
            return surfStats[index];
        }

        public void DoSurfing()
        {
            var surfWave = ClosestSurfableWave();
            if(surfWave == null)
                return;

            double dangerLeft = CheckDanger(surfWave, -1);
            double dangerRight = CheckDanger(surfWave, 1);

            double goAngle = AbsoluteBearing(surfWave.fireLocation, location);
            goAngle = dangerLeft < dangerRight
                ? WallSmoothing(location, goAngle - (Math.PI / 2), -1)
                : WallSmoothing(location, goAngle + (Math.PI / 2), 1);

            SetBackAsFront(this, goAngle);
        }

        public override void OnPaint(IGraphics graphics)
        {
            var redBrush = new SolidBrush(Color.Red);
            var rectf = new RectangleF(50, 50, 100, 150);


            //graphics.FillRectangle(redBrush, rectf);
        }

        #region Utility
        public double WallSmoothing(PointF botLocation, double angle, int orientation)
        {
            while(!fieldRect.Contains(Project(botLocation, angle, wallStick)))
            {
                angle += orientation * 0.05;
            }
            return angle;
        }

        public static PointF Project(PointF sourceLocation, double angle, double length) =>
            new PointF
            {
                X = (float)(sourceLocation.X + Math.Sin(angle) * length)
                , Y = (float)(sourceLocation.Y + Math.Cos(angle) * length)
            };
        
        public static double AbsoluteBearing(PointF source, PointF target) =>
            Math.Atan2(target.X - source.X, target.Y - source.Y);

        public static double Clamp(double value, double min, double max) =>
            Math.Max(min, Math.Min(value, max));

        public static double BulletVelocity(double power) =>
            (20.0 - (3.0 * power));

        public static double MaxEscapeAngle(double velocity) =>
            Math.Asin(8.0 / velocity);

        public static void SetBackAsFront(AdvancedRobot robot, double goAngle)
        {
            const double distance = 100;

            double angle = Utils.NormalRelativeAngle(goAngle - robot.HeadingRadians);

            if(Math.Abs(angle) > Math.PI / 2)
            {
                if(angle < 0)
                    robot.SetTurnRightRadians(Math.PI + angle);
                else
                    robot.SetTurnLeftRadians(Math.PI - angle);

                robot.SetBack(distance);
            }
            else
            {
                if(angle < 0)
                    robot.SetTurnLeftRadians(angle * -1);
                else
                    robot.SetTurnRightRadians(angle);

                robot.SetAhead(distance);
            }
        }
        #endregion
    }
}
