using CaseyDeCoder.Utilities;
using Robocode;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CaseyDeCoder.BehaviorTree
{
    public abstract class GunNodeBase : NodeBase
    {
        private const double maxPowerDistance = 150.0;
        private const double minPowerDistance = 1000.0;

        // Handles additional firing behavior that can be called by inherited gun nodes.
        protected static double Fire(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || robot.GunHeat > double.Epsilon
                || !blackboard.TryGetValue(BB.lastScannedEventKey, out ScannedRobotEvent evnt))
                return 0;

            // Decide upon the bullet power based on the distance from the enemy.
            var bulletPowerT = Utility.Map(evnt.Distance, minPowerDistance, maxPowerDistance, 0, 1);
            double bulletPower = Utility.Lerp(Rules.MIN_BULLET_POWER, Rules.MAX_BULLET_POWER, bulletPowerT);

            // Never make the bulletPower higher than the remaining victims energy
            bulletPower = Math.Min(bulletPower, evnt.Energy); 
            
            var bullet = robot.SetFireBullet(bulletPower);

            // Add the bullet to the fired bullets list.
            var bullets = blackboard.GetValue<List<Bullet>>(BB.bulletsKey);
            bullets.Add(bullet);
            blackboard.SetValue(BB.bulletsKey, bullets);

            return bullet.Power;
        }

        // Calculate the angle from the gun to the enemy.
        protected static double GunToEnemyAngle(Blackboard blackboard)
        {
            if(!blackboard.TryGetValue(BB.robotKey, out AdvancedRobot robot)
                || !blackboard.TryGetValue(BB.lastEnemyPositionKey, out Vector lastEnemyPosition))
                return default;

            var robotToEnemyAngle = Utility.Angle(robot.X, robot.Y, lastEnemyPosition.X, lastEnemyPosition.Y);

            var gunToEnemyAngle = robotToEnemyAngle - robot.GunHeading;
            gunToEnemyAngle += (gunToEnemyAngle > 180) ? -360 : (gunToEnemyAngle < -180) ? 360 : 0;

            return gunToEnemyAngle;
        }
    }
}
