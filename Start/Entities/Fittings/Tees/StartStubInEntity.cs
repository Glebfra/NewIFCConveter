using Start.API;
using Start.Attributes;

namespace Start.Entities.Fittings
{
    [StartElement(StartElementTypeEnum.STUB_IN)]
    public sealed class StartStubInEntity : StartAbstractTeeEntity
    {
        public override double HeadLength => MainDiameter / 2;
        public override double MainLength => HeadDiameter;
    }
}