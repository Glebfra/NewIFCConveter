using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Utils;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace Ifc.Geometries
{
    internal struct ConeTriangulatedGeometryProperties
    {
        public Vector<double> TopConePoint;
        public Vector<double> BottomConeCenter;
        public double Diameter;
    }

    internal struct ClippedConeTriangulatedGeometryProperties
    {
        public Vector<double> TopConeCenter;
        public Vector<double> BottomConeCenter;
        public Vector<double>? Direction;
        public double TopDiameter;
        public double BottomDiameter;
    }

    internal struct IfcTriangulatedProperties
    {
        public IEnumerable<Vector<double>> Coordinates;
        public IEnumerable<IEnumerable<int>> TriangleIndices;
        public IEnumerable<Vector<double>> Normals;

        [Pure]
        public static IfcTriangulatedProperties CreateCone(ConeTriangulatedGeometryProperties properties)
        {
            Vector<double> heightDirection = properties.TopConePoint - properties.BottomConeCenter;
            Vector<double> z = heightDirection.Normalize(2);
            Vector<double> x = z.CreateNormalVector().Normalize(2);
            Vector<double> y = z.CrossProduct(x).Normalize(2);
            Matrix<double> botMatrix = MatrixExtensions.CreateTransition(properties.BottomConeCenter, x, y, z);

            return CreateCone(properties, botMatrix);
        }

        [Pure]
        public static IfcTriangulatedProperties CreateClippedCone(ClippedConeTriangulatedGeometryProperties properties)
        {
            Vector<double> heightDirection =
                properties.Direction ?? properties.TopConeCenter - properties.BottomConeCenter;

            Vector<double> z = heightDirection.Normalize(2);
            Vector<double> x = z.CreateNormalVector().Normalize(2);
            Vector<double> y = z.CrossProduct(x).Normalize(2);
            Matrix<double> botMatrix = MatrixExtensions.CreateTransition(properties.BottomConeCenter, x, y, z);
            Matrix<double> topMatrix = MatrixExtensions.CreateTransition(properties.TopConeCenter, x, y, z);
            
            return CreateClippedCone(properties, botMatrix, topMatrix);
        }

        [Pure]
        private static IfcTriangulatedProperties CreateCone(ConeTriangulatedGeometryProperties properties,
            Matrix<double> botMatrix)
        {
            const int numSegments = 16;
            Vector<double>[] coordinates = new Vector<double>[numSegments + 1];
            Vector<double>[] normals = new Vector<double>[numSegments * 2];
            int[][] triangleIndices = new int[numSegments * 2][];

            coordinates[numSegments] = VectorExtensions.Zero;
            
            for (int i = 0; i < numSegments; i++)
            {
                double angle = 2 * Math.PI * i / numSegments;
                
                double xBot = properties.Diameter / 2 * Math.Cos(angle);
                double yBot = properties.Diameter / 2 * Math.Sin(angle);
                Vector<double> temp = new DenseVector(new double[] { xBot, yBot, 0 }).ToHomogenous();
                coordinates[i] = botMatrix.Multiply(temp).ToCartesian();
            }

            for (int i = 0; i < numSegments; i++)
            {
                triangleIndices[i] = new int[]
                {
                    (i + 0) % numSegments + 1, 
                    (i + 1) % numSegments + 1, 
                    numSegments + 1
                };
                
                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = first.CrossProduct(second).Normalize(2);
            }

            for (int i = numSegments; i < numSegments * 2; i++)
            {
                triangleIndices[i] = new int[]
                {
                    0 + 1,
                    (i + 1) % numSegments + 1,
                    (i + 2) % numSegments + 1
                };
                
                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = first.CrossProduct(second).Normalize(2);
            }

            return new IfcTriangulatedProperties
            {
                Coordinates = coordinates,
                TriangleIndices = triangleIndices,
                Normals = normals
            };
        }

        [Pure]
        private static IfcTriangulatedProperties CreateClippedCone(ClippedConeTriangulatedGeometryProperties properties,
            Matrix<double> botMatrix, Matrix<double> topMatrix)
        {
            const int numSegments = 16;
            Vector<double>[] coordinates = new Vector<double>[numSegments * 2];
            Vector<double>[] normals = new Vector<double>[numSegments * 4];
            int[][] triangleIndices = new int[numSegments * 4][];

            double botRadius = properties.BottomDiameter / 2, topRadius = properties.TopDiameter / 2;
            for (int i = 0; i < numSegments * 2; i += 2)
            {
                double angle = Math.PI * i / numSegments;
                double cos = Math.Cos(angle), sin = Math.Sin(angle);

                double xBot = botRadius * cos;
                double yBot = botRadius * sin;
                Vector<double> bottomTemp = new DenseVector(new[] { xBot, yBot, 0 }).ToHomogenous();
                coordinates[i] = botMatrix.Multiply(bottomTemp).ToCartesian();

                double xTop = topRadius * cos;
                double yTop = topRadius * sin;
                Vector<double> topTemp = new DenseVector(new[] { xTop, yTop, 0 }).ToHomogenous();
                coordinates[i + 1] = topMatrix.Multiply(topTemp).ToCartesian();
            }

            for (int i = 0; i < numSegments * 2; i += 2)
            {
                triangleIndices[i] = new int[]
                {
                    (i + 0) % (numSegments * 2) + 1,
                    (i + 2) % (numSegments * 2) + 1,
                    (i + 3) % (numSegments * 2) + 1
                };
                triangleIndices[i + 1] = new int[]
                {
                    (i + 0) % (numSegments * 2) + 1,
                    (i + 3) % (numSegments * 2) + 1,
                    (i + 1) % (numSegments * 2) + 1
                };

                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = first.CrossProduct(second).Normalize(2);

                first = coordinates[triangleIndices[i + 1][1] - 1] - coordinates[triangleIndices[i + 1][0] - 1];
                second = coordinates[triangleIndices[i + 1][2] - 1] - coordinates[triangleIndices[i + 1][1] - 1];
                normals[i + 1] = first.CrossProduct(second).Normalize(2);
            }

            for (int i = numSegments * 2; i < numSegments * 4; i += 2)
            {
                triangleIndices[i] = new int[]
                {
                    0 + 1,
                    (i + 2) % (numSegments * 2) + 1,
                    (i + 4) % (numSegments * 2) + 1
                };
                triangleIndices[i + 1] = new int[]
                {
                    1 + 1,
                    (i + 3) % (numSegments * 2) + 1,
                    (i + 5) % (numSegments * 2) + 1
                };
                
                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = first.CrossProduct(second).Normalize(2);

                first = coordinates[triangleIndices[i + 1][1] - 1] - coordinates[triangleIndices[i + 1][0] - 1];
                second = coordinates[triangleIndices[i + 1][2] - 1] - coordinates[triangleIndices[i + 1][1] - 1];
                normals[i + 1] = first.CrossProduct(second).Normalize(2);
            }

            return new IfcTriangulatedProperties
            {
                Coordinates = coordinates,
                TriangleIndices = triangleIndices,
                Normals = normals
            };
        }
    }
}