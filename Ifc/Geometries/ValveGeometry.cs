using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Ifc.API;
using Ifc.Attributes;
using Ifc.Builders.Geometry.Tessellated;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Xbim.Common;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.Interfaces;

namespace Ifc.Geometries
{
    public struct ValveGeometryProperties
    {
        public double Diameter;
        public double Length;
        
        public Vector<double> Position;
        public Vector<double> Direction;
        public Vector<double> EndDirection;
    }
    
    [IfcRepresentationIdentifier(IfcRepresentationIdentifier.Body)]
    [IfcRepresentationType(IfcRepresentationType.Tessellation)]
    public class ValveGeometry : IfcGeometry
    {
        public ValveGeometry(IIfcBuilder geometryBuilder, IIfcRepresentationContext? representationContext = null) 
            : base(geometryBuilder, representationContext)
        {
        }

        public ValveGeometry(IEnumerable<IIfcBuilder> geometryBuilders,
            IIfcRepresentationContext? representationContext = null)
            : base(geometryBuilders, representationContext)
        {
        }

        [Pure]
        public static ValveGeometry CreateGeometry(IModel model, ValveGeometryProperties properties)
        {
            Vector<double>[] directions = { properties.Direction, properties.EndDirection };
            Vector<double> topConePoint = properties.Position;
            Vector<double> firstBotConePoint = 
                topConePoint + properties.Direction.Normalize(2) * properties.Length / 2;
            Vector<double> secondBotConePoint = 
                topConePoint + properties.EndDirection.Normalize(2) * properties.Length / 2;

            IfcTriangulatedProperties[] triangulatedPropertiesArray = new IfcTriangulatedProperties[]
            {
                IfcTriangulatedProperties.CreateCone(new ConeTriangulatedGeometryProperties()
                {
                    TopConePoint = topConePoint,
                    BottomConeCenter = firstBotConePoint,
                    Diameter = properties.Diameter * 1.5,
                }),
                IfcTriangulatedProperties.CreateCone(new ConeTriangulatedGeometryProperties()
                {
                    TopConePoint = topConePoint,
                    BottomConeCenter = secondBotConePoint,
                    Diameter = properties.Diameter * 1.5,
                }),
            };
            
            IEnumerable<IIfcBuilder> builders= triangulatedPropertiesArray.Select(triangulatedProperties =>
            {
                IIfcTriangulatedFaceSetBuilder<IfcTriangulatedFaceSet> triangulatedFaceSetBuilder =
                    new IfcTriangulatedFaceSetBuilder<IfcTriangulatedFaceSet>();
                triangulatedFaceSetBuilder.CreateCoordinates(model, triangulatedProperties.Coordinates);
                triangulatedFaceSetBuilder.AssignTriangleIndices(triangulatedProperties.TriangleIndices);
                triangulatedFaceSetBuilder.AssignNormals(triangulatedProperties.Normals);

                return triangulatedFaceSetBuilder;
            });

            return new ValveGeometry(builders);
        }
    }
}