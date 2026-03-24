using System;
using System.Linq;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Geometries;
using Ifc.Interfaces;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Start.Entities.Anchors;
using Start.Interfaces;
using Utils;
using Xbim.Common;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.SharedComponentElements;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace IFCConverter.Converters.Elements
{
    public class HingedAnchorConverter : IfcElementConverter<StartHingedAnchorEntity, IfcDiscreteAccessory>
    {
        public HingedAnchorConverter(IModel model) : base(model)
        {
        }

        public override IIfcGeometry CreateGeometry(StartHingedAnchorEntity start)
        {
            IStartSegmentEntity[] segmentEntities = start.ConnectedEntities.OfType<IStartSegmentEntity>().ToArray();
            Matrix<double> segmentMatrix = segmentEntities[0].TransformationMatrix;
            double diameter = segmentEntities[0].Diameter.SIProperty;

            // Check if should double sided
            bool isDoubleSided = segmentMatrix.GetZ().IsParallel(VectorExtensions.Z);
            Vector<double> position, doubleSidedDisplacement;
            if (isDoubleSided)
            {
                position = VectorExtensions.Zero;
                doubleSidedDisplacement = segmentMatrix.GetX() * diameter;
            }
            else
            {
                double displacement = CalculateDisplacement(segmentMatrix.GetZ(), diameter);
                position = -displacement * VectorExtensions.Z;
                doubleSidedDisplacement = VectorExtensions.Zero;
            }
            HingedAnchorGeometry geometry = HingedAnchorGeometry.CreateGeometry(_Model,
                new HingedAnchorGeometryProperties
                {
                    Position = position,
                    Direction = VectorExtensions.Z,
                    Diameter = diameter,
                    IsDoubleSided = isDoubleSided,
                    DoubleSidedDisplacement = doubleSidedDisplacement
                });
            geometry.AssignColor(Color.FromHEX("4ab636"));

            return geometry;
        }

        public override Matrix<double> CreateObjectMatrix(StartHingedAnchorEntity start)
        {
            return MatrixExtensions.CreateTransition(start.Position);
        }

        public override IIfcProductBuilder<IfcDiscreteAccessory> CreateBuilder(StartHingedAnchorEntity start)
        {
            return new IfcDiscreteAccessoryBuilder<IfcDiscreteAccessory>(
                GenerateName(start), GenerateTag(start), IfcDiscreteAccessoryTypeEnum.ANCHORPLATE
            );
        }

        public override StartHingedAnchorEntity BuildStartElement(IfcDiscreteAccessory ifc)
        {
            throw new System.NotImplementedException();
        }

        private static double CalculateDisplacement(Vector<double> segmentDirection, double diameter)
        {
            double angle = segmentDirection.Angle(VectorExtensions.Z);
            if (angle.AlmostEqual(0, 1e-6)) // a=0 => sin(a)=0
                return 0;
            return diameter / (2 * Math.Sin(angle)); // r / sin(a)
        }
    }
}