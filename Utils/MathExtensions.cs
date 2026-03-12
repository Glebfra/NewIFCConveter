using System;

namespace Utils
{
    public static class MathExtensions
    {
        public static double CalculateTorusSegmentLength(double radius, double angle)
        {
            return radius * Math.Tan(angle / 2);
        }
    }
}