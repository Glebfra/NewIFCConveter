using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace Utils
{
    public static class MathExtensions
    {
        public static double CalculateTorusSegmentLength(double radius, double angle)
        {
            return radius * Math.Tan(angle / 2);
        }
        
        public static double CalculateAnchorDisplacement(Vector<double> segmentDirection, double diameter)
        {
            double angle = segmentDirection.Angle(VectorExtensions.Z);
            if (angle.AlmostEqual(0, 1e-6)) // a=0 => sin(a)=0
                return 0;
            return diameter / (2 * Math.Sin(angle)); // r / sin(a)
        }
    }
}