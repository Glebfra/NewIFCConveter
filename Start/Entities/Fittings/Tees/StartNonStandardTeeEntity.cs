using Start.API;
using Start.Attributes;

namespace Start.Entities.Fittings
{
    [StartElement(StartElementTypeEnum.NONSTANDARD_TEE)]
    public sealed class StartNonStandardTeeEntity : StartAbstractTeeEntity
    {
        public override double HeadLength => BranchHeight.SIProperty + MainDiameter / 2;
        public override double MainLength => HeadDiameter;
    }
}