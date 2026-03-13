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
    public class TorsionExpansionJointConverter : IfcElementConverter<StartTorsionExpansionJointEntity, IfcPipeFitting>
    {
        private readonly Logger _logger = Logger.GetInstance();
        
        public TorsionExpansionJointConverter(IModel model) : base(model)
        {
        }

        public override IfcPipeFitting BuildIfcElement(StartTorsionExpansionJointEntity start)
        {
            Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(start.Position);

            IStartSegmentEntity[] twoNodeEntities = start.ConnectedEntities
                .OfType<IStartSegmentEntity>()
                .ToArray();
            Vector<double>[] localPoints = twoNodeEntities
                .Select(entity => entity.GetNearestPosition(start.Position) - start.Position)
                .ToArray();
            double diameter = twoNodeEntities.Max(entity => entity.Diameter).SIProperty;

            IIfcGeometry geometry = TorsionExpansionJointGeometry.CreateGeometry(_Model,
                new TorsionExpansionJointGeometryProperties
                {
                    Diameter = diameter,
                    Points = localPoints,
                    Position = VectorExtensions.Zero
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

        public override StartTorsionExpansionJointEntity BuildStartElement(IfcPipeFitting ifc)
        {
            throw new System.NotImplementedException();
        }
    }
}