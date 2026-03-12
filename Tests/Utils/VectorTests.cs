using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using Utils;
using VectorExtensions = Utils.VectorExtensions;

namespace Tests.Utils
{
    [TestFixture]
    public class VectorTests
    {
        private const double Tolerance = 1e-10;

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

        [Test]
        public void NormalVector_ShouldBePerpendicular_ToInputVector()
        {
            Vector<double> v = Vector<double>.Build.DenseOfArray(new[] { 1.0, 2.0, 3.0 });

            Vector<double> normal = v.CreateNormalVector();

            double dot = v.DotProduct(normal);

            Assert.That(dot, Is.EqualTo(0).Within(Tolerance));
        }

        [Test]
        public void NormalVector_ShouldBeNormalized()
        {
            Vector<double> v = Vector<double>.Build.DenseOfArray(new[] { 4.0, -2.0, 1.0 });

            Vector<double> normal = v.CreateNormalVector();

            Assert.That(normal.L2Norm(), Is.EqualTo(1).Within(Tolerance));
        }

        [Test]
        public void ShouldWork_WhenVectorParallelToZ()
        {
            Vector<double> v = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 5.0 });

            Vector<double> normal = v.CreateNormalVector();

            Assert.That(v.DotProduct(normal), Is.EqualTo(0).Within(Tolerance));
            Assert.That(normal.L2Norm(), Is.EqualTo(1).Within(Tolerance));
        }

        [Test]
        public void ShouldWork_WhenVectorParallelToY()
        {
            Vector<double> v = Vector<double>.Build.DenseOfArray(new[] { 0.0, 3.0, 0.0 });

            Vector<double> normal = v.CreateNormalVector();

            Assert.That(v.DotProduct(normal), Is.EqualTo(0).Within(Tolerance));
            Assert.That(normal.L2Norm(), Is.EqualTo(1).Within(Tolerance));
        }

        [Test]
        public void ShouldReturnStableNormal_ForRandomVector()
        {
            Vector<double> v = Vector<double>.Build.DenseOfArray(new[] { 3.2, -7.1, 5.4 });

            Vector<double> normal = v.CreateNormalVector();
            Assert.That(v.DotProduct(normal), Is.EqualTo(0).Within(Tolerance));
            Assert.That(normal.L2Norm(), Is.EqualTo(1).Within(Tolerance));
        }
    }
}