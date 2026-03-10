using Ifc.Interfaces;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace Ifc.Builders.Geometry.ProfileDef
{
    public class IfcRectangleProfileDefBuilder<T> : IfcParameterizedProfileDefBuilder<T>, IIfcRectangleProfileDefBuilder<T>
        where T : IIfcRectangleProfileDef, IInstantiableEntity
    {
        public double XDim { get; }
        public double YDim { get; }
        
        public IfcRectangleProfileDefBuilder(double xDim, double yDim, IfcProfileTypeEnum profileTypeEnum, IfcLabel profileName) : base(profileTypeEnum, profileName)
        {
            XDim = xDim;
            YDim = yDim;
        }

        public override T CreateProfileDef(IModel model)
        {
            T instance = base.CreateProfileDef(model);

            const string transactionName = $"{nameof(IfcRectangleProfileDefBuilder<T>)}: {nameof(CreateProfileDef)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                instance.XDim = XDim;
                instance.YDim = YDim;
                transaction.Commit();
            }

            return instance;
        }
    }
}