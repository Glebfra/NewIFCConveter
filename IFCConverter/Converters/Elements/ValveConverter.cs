using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Geometries;
using Ifc.Interfaces;
using IFCConverter.Extensions;
using MathNet.Numerics.LinearAlgebra;
using Start.Entities.Fittings;
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
    public sealed class ValveConverter : IfcElementConverter<StartValveEntity, IfcPipeFitting>
    {
        private readonly Logger _logger = Logger.GetInstance();
        
        public ValveConverter(IModel model) : base(model)
        {
        }

        public override IfcPipeFitting BuildIfcElement(StartValveEntity start)
        {
            Vector<double> globalTopConePoint = start.Position;
            Vector<double>[] globalBotConePoints = start.GetBotConePoints();

            Vector<double> localTopConePoint = VectorExtensions.Zero;
            Vector<double>[] localBotConePoints = globalBotConePoints
                .Select(point => point - globalTopConePoint)
                .ToArray();

            IIfcGeometry valveGeometry = ValveGeometry.CreateGeometry(_Model, new ValveGeometryProperties()
            {
                Diameter = start.OutsideDiameter.SIProperty,
                Length = start.Length.SIProperty,
                TopConePoint = localTopConePoint,
                BotConePoints = localBotConePoints
            });
            valveGeometry.AssignColor(Color.FromHEX("5f4e7c"));
            _logger.Info($"Created geometry {valveGeometry.GetType().FullName}");
            
            Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(start.Position);
            _logger.Info($"Created object matrix: {objectMatrix.ToRowString()}");

            IIfcPipeFittingBuilder<IfcPipeFitting> builder =
                new IfcPipeFittingBuilder<IfcPipeFitting>(GenerateName(start), GenerateTag(start),
                    IfcPipeFittingTypeEnum.OBSTRUCTION);
            _logger.Info($"Created builder: {builder.GetType().FullName}");
            TryAddMaterial(start, builder);
            
            IIfcObjectPlacement objectPlacement = builder.CreateObjectPlacement(_Model, objectMatrix);
            builder.AssignPlacement(objectPlacement);
            builder.AssignGeometry(valveGeometry);

            return builder.CreateInstance(_Model);
        }

        public override StartValveEntity BuildStartElement(IfcPipeFitting ifc)
        {
            throw new NotImplementedException();
        }
    }
}