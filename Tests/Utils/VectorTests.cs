using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using Utils;
using VectorExtensions = Utils.VectorExtensions;

namespace Tests.Utils
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void Zero_ShouldReturnVectorWithAllComponentsZero()
        {
            Vector<double> zeroVector = VectorExtensions.Zero;
            Assert.AreEqual(0, zeroVector[0]);
            Assert.AreEqual(0, zeroVector[1]);
            Assert.AreEqual(0, zeroVector[2]);
        }

        [Test]
        public void IsParallel_ShouldReturnTrueForParallelVectors()
        {
            Vector<double> vector1 = Vector<double>.Build.DenseOfArray(new[] { 1.0, 2.0, 3.0 });
            Vector<double> vector2 = Vector<double>.Build.DenseOfArray(new[] { 2.0, 4.0, 6.0 });
            Assert.IsTrue(vector1.IsParallel(vector2));
        }

        [Test]
        public void IsParallel_ShouldReturnFalseForNonParallelVectors()
        {
            Vector<double> vector1 = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> vector2 = Vector<double>.Build.DenseOfArray(new[] { 0.0, 1.0, 0.0 });
            Assert.IsFalse(vector1.IsParallel(vector2));
        }

        [Test]
        public void IsNormal_ShouldReturnTrueForOrthogonalVectors()
        {
            Vector<double> vector1 = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> vector2 = Vector<double>.Build.DenseOfArray(new[] { 0.0, 1.0, 0.0 });
            Assert.IsTrue(vector1.IsNormal(vector2));
        }

        [Test]
        public void IsNormal_ShouldReturnFalseForNonOrthogonalVectors()
        {
            Vector<double> vector1 = Vector<double>.Build.DenseOfArray(new[] { 1.0, 1.0, 0.0 });
            Vector<double> vector2 = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Assert.IsFalse(vector1.IsNormal(vector2));
        }

        [Test]
        public void CrossProduct_ShouldReturnCorrectResultForTwoVectors()
        {
            Vector<double> vector1 = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> vector2 = Vector<double>.Build.DenseOfArray(new[] { 0.0, 1.0, 0.0 });
            Vector<double> result = vector1.CrossProduct(vector2);
            Vector<double> expected = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 1.0 });
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CreateNormalVector_ShouldReturnPerpendicularVector()
        {
            Vector<double> vector = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> normalVector = vector.CreateNormalVector();
            Assert.IsTrue(vector.IsNormal(normalVector));
        }
    }
}