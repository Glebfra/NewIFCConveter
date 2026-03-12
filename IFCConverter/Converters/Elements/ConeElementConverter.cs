using System;
using System.Linq;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Geometries;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Start.Entities.Segments;
using Utils;
using Xbim.Common;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using MatrixExtensions = Utils.MatrixExtensions;

namespace IFCConverter.Converters.Elements
{
    public sealed class ConeElementConverter : IfcElementConverter<StartConeElementEntity, IfcPipeSegment>
    {
        private readonly Logger _logger = Logger.GetInstance();

        public ConeElementConverter(IModel model) : base(model)
        {
        }

        public override IfcPipeSegment BuildIfcElement(StartConeElementEntity start)
        {
            IIfcGeometry geometry = ConeGeometry.CreateGeometry(_Model, new ConeGeometryProperties
            {
                Positions = start.Points.ToArray(),
                Diameters = start.Diameters.ToArray()
            });
            geometry.AssignColor(Color.FromHEX("46008b"));
            _logger.Info($"Created geometry {geometry.GetType().FullName}");

            Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(start.Position);
            _logger.Info($"Created object matrix: {objectMatrix.ToRowString()}");

            IIfcPipeSegmentBuilder<IfcPipeSegment> builder =
                new IfcPipeSegmentBuilder<IfcPipeSegment>(GenerateName(start), GenerateTag(start),
                    IfcPipeSegmentTypeEnum.USERDEFINED);
            _logger.Info($"Created builder: {builder.GetType().FullName}");
            TryAddMaterial(start, builder);

            IIfcObjectPlacement objectPlacement = builder.CreateObjectPlacement(_Model, objectMatrix);
            builder.AssignPlacement(objectPlacement);
            builder.AssignGeometry(geometry);

            return builder.CreateInstance(_Model);
        }

        public override StartConeElementEntity BuildStartElement(IfcPipeSegment ifc)
        {
            throw new NotImplementedException();
        }
    }
}