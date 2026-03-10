using Ifc.Interfaces;
using Xbim.Common;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace Ifc.Builders.Elements
{
    public class IfcPipeSegmentBuilder<T> : IfcFlowSegmentBuilder<T>, IIfcPipeSegmentBuilder<T>
        where T : IfcPipeSegment
    {
        public IfcPipeSegmentTypeEnum PipeSegmentTypeEnum { get; }
        
        public IfcPipeSegmentBuilder(IfcLabel name, IfcIdentifier tag, IfcPipeSegmentTypeEnum type) : base(name, tag)
        {
            PipeSegmentTypeEnum = type;
        }

        public override T CreateInstance(IModel model)
        {
            T instance = base.CreateInstance(model);

            using (ITransaction transaction = model.BeginTransaction($"{nameof(IfcPipeSegmentBuilder<T>)}: {nameof(CreateInstance)}"))
            {
                instance.PredefinedType = PipeSegmentTypeEnum;
                transaction.Commit();
            }
            
            return instance;
        }
    }
}