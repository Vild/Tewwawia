using System;
using System.Collections.Generic;
using me.WildN00b.Tewwawia.SDK;
using System.Reflection;

namespace me.WildN00b.Tewwawia.Plugin_System
{
    internal class PluginHandler
    {
        static List<Plugin> plugins = new List<Plugin>();

        public static void LoadPlugin(string path)
        {
            try
            {
                Assembly asm = Assembly.LoadFile(path);
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsClass && type.BaseType.FullName == "me.WildN00b.Tewwawia.SDK.Plugin")
                    {
                        Plugin plugin = (Plugin)asm.CreateInstance(type.FullName);
                        plugin.OnEnable();
                        plugins.Add(plugin);
                    }
                }
            }
            catch (Exception e)
            {
                Program.errorWriter.WriteLine(e.Message);
            }
        }

        public static void UnloadPlugin(Plugin plugin)
        {
            plugin.OnDisable();
            plugins.Remove(plugin);
        }

        public static void UnloadAllPlugin()
        {
            foreach (Plugin plugin in plugins)
            {
                plugin.OnDisable();
            }
            plugins.Clear();
        }

        public static void ThrowEvent(int id, params object[] args)
        {
            if (id == 0)
            {
                foreach (Plugin plugin in plugins)
                {
                    plugin.OnServerStartIsDone();
                }
            }
            else if (id == 1)
            {
                foreach (Plugin plugin in plugins)
                {
                    plugin.OnPlayerJoin((string)args[0], (string)args[1]);
                }
            }
            else
            {
                Program.errorWriter.WriteLine("Unknown Event!");
            }
        }

    }
}
