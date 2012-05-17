using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Reflection;
using me.WildN00b.Tewwawia.Plugin_System;
using me.WildN00b.Tewwawia.SDK;

namespace me.WildN00b.Tewwawia
{
    internal class Patcher
    {
        string NewPath;
        AssemblyDefinition TerrariaServer;
        MethodReference ThrowEvent;
        public Patcher(string path, string newPath)
        {
            this.NewPath = newPath;
            TerrariaServer = AssemblyDefinition.ReadAssembly(path);
            ThrowEvent = TerrariaServer.MainModule.Import(typeof(Plugin).GetMethod("ThrowEvent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
        }

        private MethodDefinition GetMethodDefinition(AssemblyDefinition file, string Class, string name, int paramcount)
        {
            TypeDefinition tdef = file.MainModule.GetType(Class);
            foreach (MethodDefinition mdef in tdef.Methods)
            {
                if ((mdef.Name == name) && (paramcount == mdef.Parameters.Count)) return mdef;
            }
            throw new ArgumentException("Unable to find this method!");
        }

        private ILProcessor GetILProcessorForMethod(AssemblyDefinition file, string Class, string name, int paramcount)
        {
            MethodDefinition DedServ = GetMethodDefinition(file, Class, name, paramcount);
            return DedServ.Body.GetILProcessor();
        }

        private void AddHook(ref ILProcessor il, int pos, int id)
        {
            MethodReference ThrowEvent = TerrariaServer.MainModule.Import(typeof(Plugin).GetMethod("ThrowEvent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
            Instruction ID = Instruction.Create(OpCodes.Ldc_I4, id);

            il.InsertAfter(il.Body.Instructions[pos], ID);
            il.InsertAfter(ID, Instruction.Create(OpCodes.Call, ThrowEvent));
        }

        private void AddHook(ref ILProcessor il, int pos, int id, Instruction[] args)
        {
            MethodReference ThrowEvent = TerrariaServer.MainModule.Import(typeof(Plugin).GetMethod("ThrowEvent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static));
            Instruction ID = Instruction.Create(OpCodes.Ldc_I4, id);

            il.InsertAfter(il.Body.Instructions[pos], ID);
            Instruction last = ID;
            foreach (Instruction a in args)
            {
                il.InsertAfter(last, a);
                last = a;
            }
            il.InsertAfter(ID, Instruction.Create(OpCodes.Call, ThrowEvent));
        }

        public void Patch()
        {
            PatchDedServ();
            PatchServerLoop();
            Save();
        }

        private void Save()
        {
            TerrariaServer.Write(NewPath);
        }

        private void PatchDedServ()
        {
            ILProcessor DedServ = GetILProcessorForMethod(TerrariaServer, "Terraria.Main", "DedServ", 0);
            
            Instruction text = Instruction.Create(OpCodes.Ldstr, "Tewwawia - terraria");
            Instruction text2 = Instruction.Create(OpCodes.Ldstr, "Tewwawia - Terraria Server ");
            DedServ.Replace(DedServ.Body.Instructions[4], text);
            DedServ.Replace(DedServ.Body.Instructions[26], text2);
            
            AddHook(ref DedServ, 488 + 2, 0);
        }

        private void PatchServerLoop()
        {
            //605
            ILProcessor ServerLoop = GetILProcessorForMethod(TerrariaServer, "Terraria.Netplay", "ServerLoop", 1);
            ArrayType ServerSock = new ArrayType(TerrariaServer.MainModule.GetType("Terraria.ServerSock").Resolve());
            List<Instruction> args = new List<Instruction>();
            args.Add(Instruction.Create(OpCodes.Ldc_I4_2));
            args.Add(Instruction.Create(OpCodes.Newarr, TerrariaServer.MainModule.Import(typeof(System.Object))));
            args.Add(Instruction.Create(OpCodes.Stloc_1));
            args.Add(Instruction.Create(OpCodes.Ldloc_1));
            args.Add(Instruction.Create(OpCodes.Ldc_I4_0));
            args.Add(Instruction.Create(OpCodes.Ldsfld, new FieldReference("Terraria.Netplay::serverSock", ServerSock)));//new TypeReference("Terraria", "ServerSock", TerrariaServer.MainModule, new Scope()))));
            args.Add(Instruction.Create(OpCodes.Ldloc_0));
            args.Add(Instruction.Create(OpCodes.Ldelem_Ref));
            args.Add(Instruction.Create(OpCodes.Ldfld, new FieldReference("Terraria.ServerSock::name", TerrariaServer.MainModule.Import(typeof(System.String)))));
            args.Add(Instruction.Create(OpCodes.Stelem_Ref));
            args.Add(Instruction.Create(OpCodes.Ldloc_1));
            args.Add(Instruction.Create(OpCodes.Ldc_I4_1));
            args.Add(Instruction.Create(OpCodes.Ldsfld, new FieldReference("Terraria.Netplay::serverSock", ServerSock)));//new ArrayType(TerrariaServer.MainModule.GetType("Terraria.ServerSock").Resolve()))));//new ArrayType(TerrariaServer.MainModule.GetType("Terraria.ServerSock")))));
            args.Add(Instruction.Create(OpCodes.Ldloc_0));
            args.Add(Instruction.Create(OpCodes.Ldelem_Ref));
            args.Add(Instruction.Create(OpCodes.Ldfld, new FieldReference("Terraria.ServerSock::tcpClient", TerrariaServer.MainModule.Import(typeof(System.Net.Sockets.TcpClient)))));
            args.Add(Instruction.Create(OpCodes.Callvirt, new MethodReference("System.Net.Sockets.TcpClient::get_Client()", TerrariaServer.MainModule.Import(typeof(System.Net.Sockets.Socket)))));
            args.Add(Instruction.Create(OpCodes.Callvirt, new MethodReference("System.Net.Sockets.Socket::get_RemoteEndPoint()", TerrariaServer.MainModule.Import(typeof(System.Net.EndPoint)))));
            args.Add(Instruction.Create(OpCodes.Stelem_Ref));
            args.Add(Instruction.Create(OpCodes.Ldloc_1));
            
            AddHook(ref ServerLoop, 605, 1, args.ToArray());
        }
    }
}
