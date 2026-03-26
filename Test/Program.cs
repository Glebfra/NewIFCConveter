using System;
using System.Collections.Generic;
using Ifc.Builders.Elements;
using Ifc.Builders.Geometry.ProfileDef;
using Ifc.Builders.Geometry.SolidModel;
using Ifc.Geometries;
using Ifc.Interfaces;
using IFCConverter;
using MathNet.Numerics.LinearAlgebra;
using Start.API;
using Xbim.Common;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.HvacDomain;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.ProfileResource;
using IfcProject = Ifc.API.IfcProject;
using MatrixExtensions = Utils.MatrixExtensions;
using VectorExtensions = Utils.VectorExtensions;

namespace Test
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (StartAutoServer startAutoServer = new())
            {
                object startDocumentRaw =
                    startAutoServer.LoadStartDocumentRaw(0x4, @"D:\Работа\Projects Files\rev1.ctp");
                IfcConverter ifcConverter = new();
                ifcConverter.Export(startDocumentRaw, 1049);
            }
        }
    }
}