using System;
using me.WildN00b.Tewwawia.SDK;

namespace me.WildN00b.Tewwawia.Plugin_System
{
    class TestPlugin : Plugin
    {
        public override void OnEnable()
        {
            Console.WriteLine("FUCK YOU TOO BERNHARD!");
        }

        public override void OnDisable()
        {
            Console.WriteLine("Gtg need to mysa kitten :)");
        }

        public override void OnServerStartIsDone()
        {
            Console.WriteLine("aaaa");
        }

        public override void OnPlayerJoin(string player, string ip)
        {
            Console.WriteLine(player + " from " + ip + "is getting kicked :)");
        }
    }
}
