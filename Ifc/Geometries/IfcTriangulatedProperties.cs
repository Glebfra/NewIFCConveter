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
    public struct ConeTriangulatedGeometryProperties
    {
        public Vector<double> TopConePoint;
        public Vector<double> BottomConeCenter;
        public double Diameter;
    }

    public struct ClippedConeTriangulatedGeometryProperties
    {
        public Vector<double> TopConeCenter;
        public Vector<double> BottomConeCenter;
        public Vector<double>? Direction;
        public double TopDiameter;
        public double BottomDiameter;
    }

    public struct IfcTriangulatedProperties
    {
        public IEnumerable<Vector<double>> Coordinates;
        public IEnumerable<IEnumerable<int>> TriangleIndices;
        public IEnumerable<Vector<double>> Normals;

        private const int _numSegments = 16;

        [Pure]
        public static IfcTriangulatedProperties CreateCone(ConeTriangulatedGeometryProperties properties)
        {
            Vector<double> heightDirection = properties.TopConePoint - properties.BottomConeCenter;
            Matrix<double> botMatrix = MatrixExtensions.CreateTransition(properties.BottomConeCenter, heightDirection);
            return CreateCone(properties, botMatrix);
        }

        [Pure]
        public static IfcTriangulatedProperties CreateClippedCone(ClippedConeTriangulatedGeometryProperties properties)
        {
            Vector<double> heightDirection = properties.Direction 
                                             ?? properties.TopConeCenter - properties.BottomConeCenter;
            Vector<double> z = heightDirection.Normalize(2);
            Matrix<double> botMatrix = MatrixExtensions.CreateTransition(properties.BottomConeCenter, z);
            Matrix<double> topMatrix = MatrixExtensions.CreateTransition(properties.TopConeCenter, z);
            return CreateClippedCone(properties, botMatrix, topMatrix);
        }

        [Pure]
        private static IfcTriangulatedProperties CreateCone(ConeTriangulatedGeometryProperties properties,
            Matrix<double> botMatrix)
        {
            Vector<double> botCenter = botMatrix.GetOffset();
            double radius = properties.Diameter / 2;

            Vector<double>[] coordinates = new Vector<double>[_numSegments + 1];
            Vector<double>[] normals = new Vector<double>[_numSegments * 2];
            int[][] triangleIndices = new int[_numSegments * 2][];

            coordinates[_numSegments] = VectorExtensions.Zero;
            
            for (int i = 0; i < _numSegments; i++)
            {
                double angle = 2 * Math.PI * i / _numSegments;
                
                double xBot = radius * Math.Cos(angle);
                double yBot = radius * Math.Sin(angle);
                Vector<double> temp = new DenseVector(new double[] { xBot, yBot, 0 });
                coordinates[i] = botMatrix.ApplyRotation(temp) + botMatrix.GetOffset();
            }

            for (int i = 0; i < _numSegments; i++)
            {
                triangleIndices[i] = new int[]
                {
                    (i + 0) % _numSegments + 1, 
                    (i + 1) % _numSegments + 1, 
                    _numSegments + 1
                };
                
                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = VectorExtensions.CreateNormalVector(first, second);
            }

            for (int i = _numSegments; i < _numSegments * 2; i++)
            {
                triangleIndices[i] = new int[]
                {
                    0 + 1,
                    (i + 1) % _numSegments + 1,
                    (i + 2) % _numSegments + 1
                };
                
                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = VectorExtensions.CreateNormalVector(first, second);
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
            Vector<double>[] coordinates = new Vector<double>[_numSegments * 2];
            Vector<double>[] normals = new Vector<double>[_numSegments * 4];
            int[][] triangleIndices = new int[_numSegments * 4][];

            double botRadius = properties.BottomDiameter / 2, topRadius = properties.TopDiameter / 2;
            for (int i = 0; i < _numSegments * 2; i += 2)
            {
                double angle = Math.PI * i / _numSegments;
                double cos = Math.Cos(angle), sin = Math.Sin(angle);

                double xBot = botRadius * cos;
                double yBot = botRadius * sin;
                Vector<double> bottomTemp = new DenseVector(new[] { xBot, yBot, 0 });
                coordinates[i] = botMatrix.ApplyRotation(bottomTemp) + botMatrix.GetOffset();

                double xTop = topRadius * cos;
                double yTop = topRadius * sin;
                Vector<double> topTemp = new DenseVector(new[] { xTop, yTop, 0 });
                coordinates[i + 1] = topMatrix.ApplyRotation(topTemp) + topMatrix.GetOffset();
            }

            for (int i = 0; i < _numSegments * 2; i += 2)
            {
                triangleIndices[i] = new int[]
                {
                    (i + 0) % (_numSegments * 2) + 1,
                    (i + 2) % (_numSegments * 2) + 1,
                    (i + 3) % (_numSegments * 2) + 1
                };
                triangleIndices[i + 1] = new int[]
                {
                    (i + 0) % (_numSegments * 2) + 1,
                    (i + 3) % (_numSegments * 2) + 1,
                    (i + 1) % (_numSegments * 2) + 1
                };

                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = VectorExtensions.CreateNormalVector(first, second);

                first = coordinates[triangleIndices[i + 1][1] - 1] - coordinates[triangleIndices[i + 1][0] - 1];
                second = coordinates[triangleIndices[i + 1][2] - 1] - coordinates[triangleIndices[i + 1][1] - 1];
                normals[i + 1] = VectorExtensions.CreateNormalVector(first, second);
            }

            for (int i = _numSegments * 2; i < _numSegments * 4; i += 2)
            {
                triangleIndices[i] = new int[]
                {
                    0 + 1,
                    (i + 2) % (_numSegments * 2) + 1,
                    (i + 4) % (_numSegments * 2) + 1
                };
                triangleIndices[i + 1] = new int[]
                {
                    1 + 1,
                    (i + 3) % (_numSegments * 2) + 1,
                    (i + 5) % (_numSegments * 2) + 1
                };
                
                Vector<double> first = coordinates[triangleIndices[i][1] - 1] - coordinates[triangleIndices[i][0] - 1];
                Vector<double> second = coordinates[triangleIndices[i][2] - 1] - coordinates[triangleIndices[i][1] - 1];
                normals[i] = VectorExtensions.CreateNormalVector(first, second);

                first = coordinates[triangleIndices[i + 1][1] - 1] - coordinates[triangleIndices[i + 1][0] - 1];
                second = coordinates[triangleIndices[i + 1][2] - 1] - coordinates[triangleIndices[i + 1][1] - 1];
                normals[i + 1] = VectorExtensions.CreateNormalVector(first, second);
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