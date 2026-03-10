using Start.API;
using Start.Attributes;

namespace Start.Entities.Fittings
{
    [StartElement(StartElementTypeEnum.SWEEPOLET)]
    public sealed class StartSweepoletEntity : StartAbstractTeeEntity
    {
        public override double HeadLength => MainDiameter / 2;
        public override double MainLength => HeadDiameter;
    }
}