using System.Collections.Generic;
using Ifc.Extensions;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.TopologyResource;

namespace Ifc.Builders.Geometry.Brep
{
    public class IfcFaceBuilder<T> : IIfcFaceBuilder<T>
        where T : IIfcFace, IInstantiableEntity
    {
        public object? Instance => IfcFace;
        public T? IfcFace { get; private set; }
        public IEnumerable<IIfcFaceBound> Bounds => _bounds;
        
        private readonly List<IIfcFaceBound> _bounds = new List<IIfcFaceBound>();

        public IIfcFaceBound CreateFaceBound(IModel model, IEnumerable<Vector<double>> points)
        {
            const string transactionName = $"{nameof(IfcFaceBuilder<T>)}: {nameof(CreateFaceBound)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IIfcFaceBound faceBound = model.Instances.New<IfcFaceBound>(bound =>
                    bound.Bound = points.ToPolyLoop(model)
                );
                _bounds.Add(faceBound);
                transaction.Commit();
                
                return faceBound;
            }
        }

        public virtual T CreateFace(IModel model)
        {
            const string transactionName = $"{nameof(IfcFaceBuilder<T>)}: {nameof(CreateFace)}";
            using (ITransaction transaction = model.BeginTransaction(transactionName))
            {
                IfcFace = model.Instances.New<T>(face => face.Bounds.AddRange(_bounds));
                transaction.Commit();
                return IfcFace;
            }
        }
        
        public object Build(IModel model)
        {
            return CreateFace(model);
        }
    }
}