using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;
using Utils;
using MatrixExtensions = Utils.MatrixExtensions;

namespace Tests.Utils
{
    [TestFixture]
    public class MatrixTests
    {
        private const double Tolerance = 1e-10;
        
        private static Vector<double> V(double x, double y, double z) =>
            Vector<double>.Build.DenseOfArray(new[] { x, y, z });

        [Test]
        public void Identity_ShouldReturnIdentity()
        {
            Matrix<double> m = MatrixExtensions.Identity();

            Assert.That(m.RowCount, Is.EqualTo(4));
            Assert.That(m.ColumnCount, Is.EqualTo(4));

            for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                Assert.That(m[i, j], Is.EqualTo(i == j ? 1 : 0).Within(Tolerance));
        }

        [Test]
        public void ParallelAxes_ShouldProduceInvalidZ()
        {
            Vector<double> pos = V(0, 0, 0);
            Vector<double> x = V(1, 0, 0);
            Vector<double> y = V(2, 0, 0); // параллельная ось

            Matrix<double> m = MatrixExtensions.CreateTransition(pos, x, y);

            Vector<double> z = m.GetZ();

            // z будет нулевым вектором после нормализации
            Assert.That(z.L2Norm(), Is.EqualTo(0).Within(Tolerance));
        }

        [Test]
        public void ZeroAxis_ShouldProduceNaN()
        {
            Vector<double> pos = V(0, 0, 0);
            Vector<double> x = V(0, 0, 0);

            Matrix<double> m = MatrixExtensions.CreateTransition(pos, x);

            Vector<double> y = m.GetY();

            Assert.That(double.IsNaN(y[0]) || y.L2Norm() < Tolerance);
        }
        
        [Test]
        public void Identity_ShouldReturn4x4IdentityMatrix()
        {
            Matrix<double> identityMatrix = MatrixExtensions.Identity();
            Assert.AreEqual(4, identityMatrix.RowCount);
            Assert.AreEqual(4, identityMatrix.ColumnCount);
            Assert.IsTrue(identityMatrix.Equals(Matrix<double>.Build.DenseIdentity(4)));
        }
        
        [Test]
        public void GetX_ShouldReturnFirstRowSubVector()
        {
            Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
                { 13, 14, 15, 16 }
            });
            Vector<double> xVector = matrix.GetX();
            Assert.AreEqual(Vector<double>.Build.DenseOfArray(new[] { 1.0, 2.0, 3.0 }), xVector);
        }
        
        [Test]
        public void GetOffset_ShouldReturnLastRowSubVector()
        {
            Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
                { 13, 14, 15, 16 }
            });
            Vector<double> offsetVector = matrix.GetOffset();
            Assert.AreEqual(Vector<double>.Build.DenseOfArray(new double[] { 4, 8, 12 }), offsetVector);
        }
        
        [Test]
        public void SetX_ShouldUpdateFirstRowWithGivenVector()
        {
            Matrix<double> matrix = Matrix<double>.Build.Dense(4, 4);
            Vector<double> vector = Vector<double>.Build.DenseOfArray(new[] { 1.0, 2.0, 3.0 });
            matrix.SetX(vector);
            Assert.AreEqual(vector, matrix.GetX());
        }
        
        [Test]
        public void CreateTransition_ShouldReturnMatrixWithCorrectPositionAndAxes()
        {
            Vector<double> position = Vector<double>.Build.DenseOfArray(new[] { 1.0, 2.0, 3.0 });
            Vector<double> xAxis = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> yAxis = Vector<double>.Build.DenseOfArray(new[] { 0.0, 1.0, 0.0 });
            Vector<double> zAxis = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 1.0 });
        
            Matrix<double> transitionMatrix = MatrixExtensions.CreateTransition(position, xAxis, yAxis, zAxis);
        
            Assert.AreEqual(position, transitionMatrix.GetOffset());
            Assert.AreEqual(xAxis, transitionMatrix.GetX());
            Assert.AreEqual(yAxis, transitionMatrix.GetY());
            Assert.AreEqual(zAxis, transitionMatrix.GetZ());
        }
        
        [Test]
        public void CreateTransition_ShouldHandleSingleAxisInput()
        {
            Vector<double> position = Vector<double>.Build.DenseOfArray(new[] { 1.0, 2.0, 3.0 });
            Vector<double> zAxis = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
        
            Matrix<double> transitionMatrix = MatrixExtensions.CreateTransition(position, zAxis);
        
            Assert.AreEqual(position, transitionMatrix.GetOffset());
            Assert.AreEqual(zAxis, transitionMatrix.GetZ());
            Assert.IsTrue(transitionMatrix.GetX().IsNormal(transitionMatrix.GetZ()));
            Assert.IsTrue(transitionMatrix.GetY().IsNormal(transitionMatrix.GetZ()));
        }
        
        [Test]
        public void Should_Set_Translation()
        {
            Vector<double> position = Vector<double>.Build.DenseOfArray(new[] { 10.0, 20.0, 30.0 });

            Vector<double> x = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> y = Vector<double>.Build.DenseOfArray(new[] { 0.0, 1.0, 0.0 });
            Vector<double> z = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 1.0 });

            Matrix<double> matrix = MatrixExtensions.CreateTransition(position, x, y, z);

            Assert.That(matrix.GetOffset(), Is.EqualTo(position));
        }

        [Test]
        public void Should_Normalize_Axes()
        {
            Vector<double> position = Vector<double>.Build.Dense(3);

            Vector<double> x = Vector<double>.Build.DenseOfArray(new[] { 10.0, 0.0, 0.0 });
            Vector<double> y = Vector<double>.Build.DenseOfArray(new[] { 0.0, 5.0, 0.0 });
            Vector<double> z = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, -2.0 });

            Matrix<double> matrix = MatrixExtensions.CreateTransition(position, x, y, z);

            Assert.That(matrix.GetX().L2Norm(), Is.EqualTo(1).Within(Tolerance));
            Assert.That(matrix.GetY().L2Norm(), Is.EqualTo(1).Within(Tolerance));
            Assert.That(matrix.GetZ().L2Norm(), Is.EqualTo(1).Within(Tolerance));
        }

        [Test]
        public void Should_Store_Correct_Axes()
        {
            Vector<double> position = Vector<double>.Build.Dense(3);

            Vector<double> x = Vector<double>.Build.DenseOfArray(new[] { 2.0, 0.0, 0.0 });
            Vector<double> y = Vector<double>.Build.DenseOfArray(new[] { 0.0, 3.0, 0.0 });
            Vector<double> z = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 4.0 });

            Matrix<double> matrix = MatrixExtensions.CreateTransition(position, x, y, z);

            Assert.That(matrix.GetX(), Is.EqualTo(x.Normalize(2)));
            Assert.That(matrix.GetY(), Is.EqualTo(y.Normalize(2)));
            Assert.That(matrix.GetZ(), Is.EqualTo(z.Normalize(2)));
        }

        [Test]
        public void Should_Create_4x4_Matrix()
        {
            Vector<double> position = Vector<double>.Build.Dense(3);

            Vector<double> x = Vector<double>.Build.DenseOfArray(new[] { 1.0, 0.0, 0.0 });
            Vector<double> y = Vector<double>.Build.DenseOfArray(new[] { 0.0, 1.0, 0.0 });
            Vector<double> z = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 1.0 });

            Matrix<double> matrix = MatrixExtensions.CreateTransition(position, x, y, z);

            Assert.That(matrix.RowCount, Is.EqualTo(4));
            Assert.That(matrix.ColumnCount, Is.EqualTo(4));
        }

        [Test]
        public void Should_Not_Modify_Input_Vectors()
        {
            Vector<double> position = Vector<double>.Build.Dense(3);

            Vector<double> x = Vector<double>.Build.DenseOfArray(new[] { 3.0, 0.0, 0.0 });
            Vector<double> y = Vector<double>.Build.DenseOfArray(new[] { 0.0, 4.0, 0.0 });
            Vector<double> z = Vector<double>.Build.DenseOfArray(new[] { 0.0, 0.0, 5.0 });

            Vector<double> xCopy = x.Clone();
            Vector<double> yCopy = y.Clone();
            Vector<double> zCopy = z.Clone();

            MatrixExtensions.CreateTransition(position, x, y, z);
            
            Assert.That(x, Is.EqualTo(xCopy));
            Assert.That(y, Is.EqualTo(yCopy));
            Assert.That(z, Is.EqualTo(zCopy));
        }
    }
}