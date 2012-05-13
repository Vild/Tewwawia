using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using me.WildN00b.Tewwawia.Plugin_System;

namespace me.WildN00b.Tewwawia.SDK
{
    public abstract class Plugin
    {
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void OnServerStartIsDone();
        public abstract void OnPlayerJoin(string player, string ip);

        public static void ThrowEvent(int id, params object[] args)
        {
            PluginHandler.ThrowEvent(id, args);
        }
    }
}
