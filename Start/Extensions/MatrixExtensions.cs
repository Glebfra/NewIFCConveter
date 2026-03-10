using MathNet.Numerics.LinearAlgebra;
using Utils;

namespace Start.Extensions
{
    internal static class MatrixExtensions
    {
        public static Matrix<double> CreateSegmentTransitionMatrix(Vector<double> position, Vector<double> direction)
        {
            Vector<double> xm = direction.Normalize(2);
            
            Vector<double> arbitraryVector = Utils.VectorExtensions.Z;
            Vector<double> secondArbitraryVector = xm.CrossProduct(arbitraryVector).Normalize(2);
            Vector<double> zm = xm.CrossProduct(secondArbitraryVector).Normalize(2);
            if (zm.DotProduct(Utils.VectorExtensions.Z) < 0)
                zm = zm.Negate();
            Vector<double> ym = zm.CrossProduct(xm).Normalize(2);

            return Utils.MatrixExtensions.CreateTransition(position, ym, zm, xm);
        }
    }
}