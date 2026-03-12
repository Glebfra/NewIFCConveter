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
            using (StartAutoServer startAutoServer = new StartAutoServer())
            {
                object startDocumentRaw = startAutoServer.LoadStartDocumentRaw(0x4, @"D:\Работа\Projects Files\AngularExpansionJointTest.ctp");
                IfcConverter ifcConverter = new IfcConverter();
                ifcConverter.Export(startDocumentRaw, 1049);
            }
        }
    }
}