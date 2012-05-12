using System;
using System.Reflection;
using System.IO;

namespace me.WildN00b.Tewwawia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int ret = new Program().Run(args);
#if DEBUG
            Console.WriteLine((ret == 0) ? "EVERYTHING IS OKEY!!!" : "THE WORLD IS GOING TO END!!! ret: " + ret);
            Console.ReadLine();
#endif
        }

        public static string TERRARIASERVER = Environment.CurrentDirectory + "\\TerrariaServer.exe";
        public static string TERRARIASERVER_PATCHED = Environment.CurrentDirectory + "\\Tewwawia.dat";
        public TextWriter errorWriter = Console.Error;
        public Assembly TerrariaServer;

        private int Run(string[] args)
        {
            if (!File.Exists(TERRARIASERVER))
            {
                errorWriter.WriteLine("'TerrariaServer.exe' not found, need to be in the same folder as Tewwawia.exe!");
                return -1;
            }

            Patcher patch = new Patcher(TERRARIASERVER, TERRARIASERVER_PATCHED);
            patch.Patch();

            if (!Load())
                return -2;
            Console.ForegroundColor = ConsoleColor.Green; // Make terraria 200% cooler
            TerrariaServer.EntryPoint.Invoke(null, new object[] { args });

            return 0;
        }

        private bool Load()
        {
            try
            {
                TerrariaServer = Assembly.LoadFile(TERRARIASERVER_PATCHED);
            }
            catch (Exception e)
            {
                errorWriter.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}
