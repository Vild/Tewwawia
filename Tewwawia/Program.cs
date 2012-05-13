using System;
using System.Reflection;
using System.IO;
using me.WildN00b.Tewwawia.Plugin_System;

namespace me.WildN00b.Tewwawia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
#if DEBUG
            Console.ReadLine();
#endif
        }

        public static string TERRARIASERVER = Environment.CurrentDirectory + "\\TerrariaServer.exe";
        public static string TERRARIASERVER_PATCHED = Environment.CurrentDirectory + "\\Tewwawia.dat";
        public static TextWriter errorWriter = Console.Error;
        public Assembly TerrariaServer;

        private int Run(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Make terraria 200% cooler
            if (!File.Exists(TERRARIASERVER))
            {
                errorWriter.WriteLine("'TerrariaServer.exe' not found, need to be in the same folder as Tewwawia.exe!");
                return -1;
            }

            Patch();
            
            if (!Load())
                return -2;

            PluginHandler.LoadPlugin(Environment.CurrentDirectory + "\\Tewwawia.exe");

            TerrariaServer.EntryPoint.Invoke(null, new object[] { args });

            PluginHandler.UnloadAllPlugin();
            return 0;
        }

        private static void Patch()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Need to patch.");
            Patcher patch = new Patcher(TERRARIASERVER, TERRARIASERVER_PATCHED);
            Console.Write("Pathing...");
            patch.Patch();
            Console.WriteLine(" done!");
            Console.ForegroundColor = ConsoleColor.Green;
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
