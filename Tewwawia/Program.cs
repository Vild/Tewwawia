using System;
using System.Reflection;
using System.IO;

namespace Tewwawia
{
    class Program
    {
        static void Main(string[] args)
        {
            int ret = new Program().Run(args);
#if DEBUG
            Console.WriteLine((ret == 0) ? "EVERYTHING IS OKEY!!!" : "THE WORLD IS GOING TO END!!! ret: " + ret);
            Console.ReadLine();
#endif
        }

        public TextWriter errorWriter = Console.Error;
        public Assembly TerrariaServer;

        private int Run(string[] args)
        {
            if (!Load())
                return -1;
            Console.ForegroundColor = ConsoleColor.Green; // Make terraria 200% cooler
            TerrariaServer.EntryPoint.Invoke(null, new object[] { args });

            return 0;
        }

        private bool Load()
        {
            try
            {
                if (!File.Exists(Environment.CurrentDirectory + "/TerrariaServer.exe"))
                    throw new FileNotFoundException("'TerrariaServer.exe' not found, need to be in the same folder as Tewwawia.exe!");
                TerrariaServer = Assembly.LoadFile(Environment.CurrentDirectory + "/TerrariaServer.exe");
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
