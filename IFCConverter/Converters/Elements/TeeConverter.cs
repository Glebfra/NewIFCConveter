using System;
using System.Linq;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Geometries;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Start.Entities.Fittings;
using Start.Extensions;
using Utils;
using Xbim.Common;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace IFCConverter.Converters.Elements
{
    public sealed class TeeConverter : IfcElementConverter<StartAbstractTeeEntity, IfcPipeFitting>
    {
        private readonly Logger _logger = Logger.GetInstance();

        public TeeConverter(IModel model) : base(model)
        {
        }

        public override IfcPipeFitting BuildIfcElement(StartAbstractTeeEntity start)
        {
            IIfcGeometry teeGeometry = TeeGeometry.CreateGeometry(_Model, new TeeGeometryProperties
            {
                HeadDiameter = start.HeadDiameter,
                HeadDirection = start.HeadSegment.GetProjectionFromPoint(start.Position),
                HeadLength = start.HeadLength,

                MainDiameter = start.MainDiameter,
                MainDirection = start.MainSegments.ElementAt(0).GetProjectionFromPoint(start.Position).Negate(),
                MainLength = start.MainLength,

                Position = VectorExtensions.Zero
            });
            teeGeometry.AssignColor(Color.FromHEX("5f4e7c"));
            _logger.Info($"Created geometry {teeGeometry.GetType().FullName}");

            Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(start.Position);
            _logger.Info($"Created object matrix: {objectMatrix.ToRowString()}");

            IIfcPipeFittingBuilder<IfcPipeFitting> builder =
                new IfcPipeFittingBuilder<IfcPipeFitting>(GenerateName(start), GenerateTag(start),
                    IfcPipeFittingTypeEnum.JUNCTION);
            _logger.Info($"Created builder: {builder.GetType().FullName}");
            TryAddMaterial(start, builder);

            IIfcObjectPlacement objectPlacement = builder.CreateObjectPlacement(_Model, objectMatrix);
            builder.AssignPlacement(objectPlacement);
            builder.AssignGeometry(teeGeometry);

            return builder.CreateInstance(_Model);
        }

        public override StartAbstractTeeEntity BuildStartElement(IfcPipeFitting ifc)
        {
            throw new NotImplementedException();
        }
    }
}