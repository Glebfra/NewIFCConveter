using Ifc.Interfaces;
using Xbim.Common;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace Ifc.Builders.Elements
{
    public class IfcPipeFittingBuilder<T> : IfcFlowElementBuilder<T>, IIfcPipeFittingBuilder<T>
        where T : IfcPipeFitting
    {
        public IfcPipeFittingTypeEnum PipeFittingType { get; }
        
        public IfcPipeFittingBuilder(IfcLabel name, IfcIdentifier tag, IfcPipeFittingTypeEnum type) : base(name, tag)
        {
            PipeFittingType = type;
        }

        public override T CreateInstance(IModel model)
        {
            T instance = base.CreateInstance(model);

            using (ITransaction transaction = model.BeginTransaction($"{nameof(IfcPipeFittingBuilder<T>)}: {nameof(CreateInstance)}"))
            {
                instance.PredefinedType = PipeFittingType;
                transaction.Commit();
            }

            return instance;
        }
    }
}