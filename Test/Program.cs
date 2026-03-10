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
            using (StartAutoServer startAutoServer = new StartAutoServer())
            {
                object startDocumentRaw = startAutoServer.LoadStartDocumentRaw(0x4, @"D:\Работа\Projects Files\ReducersTest.ctp");
                IfcConverter ifcConverter = new IfcConverter();
                ifcConverter.Export(startDocumentRaw, 1049);
            }
        }
    }
}