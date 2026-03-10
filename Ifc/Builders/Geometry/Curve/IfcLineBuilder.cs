using System;
using Ifc.Extensions;
using Ifc.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;

namespace Ifc.Builders.Geometry.Curve
{
    public class IfcLineBuilder<T> : IfcCurveBuilder<T>, IIfcLineBuilder<T>
        where T : IIfcLine, IInstantiableEntity
    {
        public IIfcCartesianPoint? Point { get; private set; }
        public IIfcVector? Direction { get; private set; }

        public IIfcCartesianPoint CreatePoint(IModel model, Vector<double> point)
        {
            using (ITransaction transaction =
                   model.BeginTransaction($"{nameof(IfcLineBuilder<T>)}: {nameof(CreatePoint)}"))
            {
                Point = point.ToCartesianPoint(model);
                transaction.Commit();

                return Point;
            }
        }

        public IIfcVector CreateDirection(IModel model, Vector<double> vector)
        {
            using (ITransaction transaction =
                   model.BeginTransaction($"{nameof(IfcLineBuilder<T>)}: {nameof(CreateDirection)}"))
            {
                Direction = vector.ToIfcVector(model);
                transaction.Commit();

                return Direction;
            }
        }

        public override T CreateCurve(IModel model)
        {
            if (Point == null)
                throw new NullReferenceException(
                    $"{nameof(IfcLineBuilder<T>)}: {nameof(Point)}. Call {nameof(CreatePoint)} before {nameof(CreateCurve)}");
            if (Direction == null)
                throw new NullReferenceException(
                    $"{nameof(IfcLineBuilder<T>)}: {nameof(Direction)}. Call {nameof(CreateDirection)} before {nameof(CreateCurve)}");

            T curve = base.CreateCurve(model);

            using (ITransaction transaction =
                   model.BeginTransaction($"{nameof(IfcLineBuilder<T>)} : {nameof(CreateCurve)}"))
            {
                curve.Pnt = Point;
                curve.Dir = Direction;
                transaction.Commit();

                return curve;
            }
        }
    }
}