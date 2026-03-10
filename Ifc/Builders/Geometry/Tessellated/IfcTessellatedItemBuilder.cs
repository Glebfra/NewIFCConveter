using Ifc.Interfaces;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace Ifc.Builders.Geometry.Tessellated
{
    public class IfcTessellatedItemBuilder<T> : IIfcTessellatedItemBuilder<T>
        where T : IIfcTessellatedItem, IInstantiableEntity
    {
        public T? TessellatedItem { get; private set; }

        public object? Instance => TessellatedItem;
        
        public virtual T CreateTessellatedItem(IModel model)
        {
            const string transactionName = $"{nameof(IfcTessellatedItemBuilder<T>)}: {nameof(CreateTessellatedItem)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                TessellatedItem = model.Instances.New<T>();
                transaction.Commit();

                return TessellatedItem;
            }
        }
        
        public object Build(IModel model)
        {
            return CreateTessellatedItem(model);
        }
    }
}