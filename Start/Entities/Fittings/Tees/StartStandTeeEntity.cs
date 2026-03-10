using Start.API;
using Start.Attributes;

namespace Start.Entities.Fittings
{
    [StartElement(StartElementTypeEnum.STAND_TEE)]
    public sealed class StartStandTeeEntity : StartAbstractTeeEntity
    {
        public override double HeadLength => BranchHeight.SIProperty + MainDiameter / 2;
        public override double MainLength => HeaderLength.SIProperty;
    }
}