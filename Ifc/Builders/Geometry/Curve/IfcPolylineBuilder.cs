using System;
using System.Collections.Generic;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace Ifc.Builders.Geometry.Curve
{
    public class IfcPolylineBuilder<T> : IfcBoundedCurveBuilder<T>, IIfcPolylineBuilder<T>
        where T : IIfcPolyline, IInstantiableEntity
    {
        public IEnumerable<IIfcCartesianPoint>? Points { get; private set; }
        
        public void CreatePoints(IModel model, IEnumerable<Vector<double>> points)
        {
            const string transactionName = $"{nameof(IfcPolylineBuilder<T>)}: {nameof(CreatePoints)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                
            }
            throw new NotImplementedException();
        }
    }
}