using System;
using System.Collections.Generic;
using System.Linq;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Geometries;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Start.Entities.ExpansionJoints;
using Start.Extensions;
using Start.Interfaces;
using Utils;
using Xbim.Common;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace IFCConverter.Converters.Elements
{
    public class SphericalPipesJointConverter : IfcElementConverter<StartAngularExpansionJointEntity, IfcPipeFitting>
    {
        private readonly Logger _logger = Logger.GetInstance();

        public SphericalPipesJointConverter(IModel model) : base(model)
        {
        }

        public override IfcPipeFitting BuildIfcElement(StartAngularExpansionJointEntity start)
        {
            Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(start.Position);

            IStartSegmentEntity[] startSegmentEntities =
                start.ConnectedEntities.OfType<IStartSegmentEntity>().ToArray();
            IEnumerable<Vector<double>> globalPoints = startSegmentEntities
                .Select(segment => segment.GetNearestPosition(start.Position));
            Vector<double>[] localPoints = globalPoints.Select(point => point - start.Position).ToArray();

            double diameter = startSegmentEntities.Max(segment => segment.Diameter).SIProperty;
            SphericalPipesJointGeometry geometry = SphericalPipesJointGeometry.CreateGeometry(_Model,
                new SphericalPipesJointGeometryProperties
                {
                    Position = VectorExtensions.Zero,
                    Points = localPoints,
                    PipeDiameter = diameter,
                    SphereDiameter = diameter * 1.5,
                    Length = start.Length.SIProperty
                });
            geometry.AssignColor(Color.FromHEX("5f4e7c"));
            _logger.Info($"Created geometry {geometry.GetType().FullName}");

            IIfcPipeFittingBuilder<IfcPipeFitting> builder = new IfcPipeFittingBuilder<IfcPipeFitting>(
                GenerateName(start), GenerateTag(start), IfcPipeFittingTypeEnum.CONNECTOR
            );
            _logger.Info($"Created builder: {builder.GetType().FullName}");
            TryAddMaterial(start, builder);

            builder.AssignGeometry(geometry);
            builder.CreateObjectPlacement(_Model, objectMatrix);

            return builder.CreateInstance(_Model);
        }

        public override StartAngularExpansionJointEntity BuildStartElement(IfcPipeFitting ifc)
        {
            throw new NotImplementedException();
        }
    }
}