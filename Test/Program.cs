using System;
using Ifc.API;
using Ifc.Builders.Elements;
using Ifc.Builders.Geometry.Tessellated;
using Ifc.Geometries;
using Ifc.Interfaces;
using IFCConverter;
using MathNet.Numerics.LinearAlgebra;
using Start.API;
using Xbim.Common;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace Test
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            /*using (IfcProject ifcProject = IfcProject.CreateProject("Test project"))
            {
                IModel model = ifcProject.Model;
                
                Matrix<double> objectMatrix = MatrixExtensions.CreateTransition(
                    VectorExtensions.Zero,
                    VectorExtensions.X
                );

                ConeGeometry coneGeometry = ConeGeometry.CreateGeometry(model, new ConeGeometryProperties
                {
                    Diameters = new double[] { 1.0, 0.5 },
                    Direction = VectorExtensions.Z,
                    Positions = new Vector<double>[] { VectorExtensions.Zero, VectorExtensions.Z }
                });

                IIfcPipeFittingBuilder<IfcPipeFitting> builder =
                    new IfcPipeFittingBuilder<IfcPipeFitting>("Test", "Test", IfcPipeFittingTypeEnum.CONNECTOR);
                builder.CreateObjectPlacement(model, objectMatrix);
                builder.AssignGeometry(coneGeometry);

                IfcPipeFitting pipeFitting = builder.CreateInstance(model);
                ifcProject.AddEntityRaw(pipeFitting);
                
                ifcProject.SaveAs(@"D:\Работа\Projects Files\IfcTest.ifc");
            }*/

            using (StartAutoServer startAutoServer = new StartAutoServer())
            {
                object startDocumentRaw = startAutoServer.LoadStartDocumentRaw(0x4, @"D:\Работа\Projects Files\ReducersTest.ctp");
                IfcConverter ifcConverter = new IfcConverter();
                ifcConverter.Export(startDocumentRaw, 1049);
            }
        }
    }
}