using System;
using System.Linq;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Geometries;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Start.Entities.Fittings;
using Utils;
using Xbim.Common;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace IFCConverter.Converters.Elements
{
    public sealed class ReducerConverter : IfcElementConverter<StartAbstractReducerEntity, IfcPipeFitting>
    {
        private readonly Logger _logger = Logger.GetInstance();

        public ReducerConverter(IModel model) : base(model)
        {
        }

        public override IfcPipeFitting BuildIfcElement(StartAbstractReducerEntity start)
        {
            Vector<double> forward = (
                start.Points.ElementAt(1) - start.Points.ElementAt(0)
            ).DotProduct(start.SegmentWithMinDiameter.Projection) * start.SegmentWithMinDiameter.Projection;
            Matrix<double> transitionMatrix = MatrixExtensions.CreateTransitionWithWorldUp(start.Position, forward);

            IIfcGeometry geometry = ConeGeometry.CreateGeometry(_Model, new ConeGeometryProperties
            {
                Direction = VectorExtensions.Forward,
                Positions = start.Points.ToArray(),
                Diameters = start.Diameters.ToArray()
            });
            geometry.AssignColor(Color.FromHEX("5f4e7c"));
            _logger.Info($"Created geometry {geometry.GetType().FullName}");

            Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(start.Position);
            _logger.Info($"Created object matrix: {objectMatrix.ToRowString()}");

            IIfcPipeFittingBuilder<IfcPipeFitting> builder =
                new IfcPipeFittingBuilder<IfcPipeFitting>(GenerateName(start), GenerateTag(start),
                    IfcPipeFittingTypeEnum.CONNECTOR);
            _logger.Info($"Created builder: {builder.GetType().FullName}");
            TryAddMaterial(start, builder);

            IIfcObjectPlacement objectPlacement = builder.CreateObjectPlacement(_Model, transitionMatrix);
            builder.AssignPlacement(objectPlacement);
            builder.AssignGeometry(geometry);

            return builder.CreateInstance(_Model);
        }

        public override StartAbstractReducerEntity BuildStartElement(IfcPipeFitting ifc)
        {
            throw new NotImplementedException();
        }
    }
}