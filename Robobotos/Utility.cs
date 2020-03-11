using Robocode.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseyDeCoder.Utilities
{
    public static class Utility
    {
        // Calculates the angle between 2 points and converts it to a robocode compatable heading
        public static double Angle(double x1, double y1, double x2, double y2)
        {
            // Return success if the angle left to turn is less than the angleMargin
            var angle = Math.Atan2(y1 - y2, x1 - x2);
            var angleDegrees = Utils.ToDegrees(angle);
            var robocodeAngle = ((angleDegrees * -1) + 270) % 360;

            return robocodeAngle;
        }

        // Math functions for ease of use
        public static double Clamp(double value, double min, double max) => Math.Min(Math.Max(value, min), max);
        public static double Clamp01(double value) => Clamp(value, 0, 1);
        public static double LerpUnclamped(double a, double b, double t) => a * (1 - t) + b * t;
        public static double Lerp(double a, double b, double t) => LerpUnclamped(a, b, Clamp01(t));
        public static double Map(double value, double inMin, double inMax, double outMin, double outMax) => (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        public static double DistanceSquared(double x1, double y1, double x2, double y2) => Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2);
        public static double Distance(double x1, double y1, double x2, double y2) => Math.Sqrt(DistanceSquared(x1, y1, x2, y2));
    }
}
