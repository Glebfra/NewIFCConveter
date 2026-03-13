using System;
using IFCConverter;
using Start.API;

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
                    startAutoServer.LoadStartDocumentRaw(0x4,
                        @"D:\Работа\Projects Files\rev1.ctp");
                IfcConverter ifcConverter = new();
                ifcConverter.Export(startDocumentRaw, 1049);
            }
        }
    }
}