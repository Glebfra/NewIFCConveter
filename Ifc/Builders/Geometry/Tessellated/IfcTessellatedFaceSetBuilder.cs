using System.Collections.Generic;
using Ifc.Extensions;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace Ifc.Builders.Geometry.Tessellated
{
    public class IfcTessellatedFaceSetBuilder<T> : IfcTessellatedItemBuilder<T>, IIfcTessellatedFaceSetBuilder<T>
        where T : IIfcTessellatedFaceSet, IInstantiableEntity
    {
        public IIfcCartesianPointList3D? Coordinates { get; private set; }
        
        public IIfcCartesianPointList3D CreateCoordinates(IModel model, IEnumerable<Vector<double>> coordinates)
        {
            const string transactionName = $"{nameof(IfcTessellatedFaceSetBuilder<T>)}: {nameof(CreateCoordinates)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                Coordinates = coordinates.ToCartesianPointList3D(model);
                transaction.Commit();

                return Coordinates;
            }
        }

        public override T CreateTessellatedItem(IModel model)
        {
            T instance = base.CreateTessellatedItem(model);

            const string transactionName = $"{nameof(IfcTessellatedFaceSetBuilder<T>)}: {nameof(CreateTessellatedItem)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                instance.Coordinates = Coordinates;
                transaction.Commit();
            }

            return instance;
        }
    }
}