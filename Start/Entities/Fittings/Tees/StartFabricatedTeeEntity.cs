using Start.API;
using Start.Attributes;

namespace Start.Entities.Fittings
{
    [StartElement(StartElementTypeEnum.FABRICATED_TEE)]
    public sealed class StartFabricatedTeeEntity : StartAbstractTeeEntity
    {
        public override double HeadLength => BranchHeight.SIProperty + MainDiameter / 2;
        public override double MainLength => HeaderLength.SIProperty;
    }
}