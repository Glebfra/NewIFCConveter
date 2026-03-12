using System;
using Ifc.Extensions;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Xbim.Common;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.SharedBldgServiceElements;

namespace Ifc.Builders
{
    public class IfcPortBuilder : IIfcPortBuilder
    {
        public IfcPortBuilder(
            IfcDistributionPortTypeEnum distributionPortTypeEnum = IfcDistributionPortTypeEnum.NOTDEFINED,
            IfcFlowDirectionEnum flowDirectionEnum = IfcFlowDirectionEnum.NOTDEFINED)
        {
            DistributionPortTypeEnum = distributionPortTypeEnum;
            FlowDirectionEnum = flowDirectionEnum;
        }

        public object? Instance => IfcPort;

        public IIfcPort? IfcPort { get; private set; }
        public IIfcObjectPlacement? ObjectPlacement { get; private set; }

        public IfcDistributionPortTypeEnum DistributionPortTypeEnum { get; }
        public IfcFlowDirectionEnum FlowDirectionEnum { get; }

        public IIfcPort CreatePort(IModel model)
        {
            if (ObjectPlacement == null)
                throw new NullReferenceException(
                    $"{nameof(IfcPortBuilder)}: {nameof(ObjectPlacement)}. Call {nameof(CreateObjectPlacement)} before {nameof(CreatePort)}");

            using (ITransaction transaction = model.BeginTransaction($"{nameof(IfcPortBuilder)}: {nameof(CreatePort)}"))
            {
                IfcPort = model.Instances.New<IfcDistributionPort>(port =>
                {
                    port.PredefinedType = DistributionPortTypeEnum;
                    port.FlowDirection = FlowDirectionEnum;
                    port.ObjectPlacement = (IfcObjectPlacement)ObjectPlacement;
                });
                transaction.Commit();

                return IfcPort;
            }
        }

        public IIfcObjectPlacement CreateObjectPlacement(IModel model, Matrix<double> matrix3D)
        {
            using (ITransaction transaction =
                   model.BeginTransaction($"{nameof(IfcPortBuilder)}: {nameof(CreateObjectPlacement)}"))
            {
                ObjectPlacement = matrix3D.ToIfcObjectPlacement(model);
                transaction.Commit();

                return ObjectPlacement;
            }
        }

        public object Build(IModel model)
        {
            return CreatePort(model);
        }
    }
}