using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace me.WildN00b.Tewwawia
{
    class Patcher
    {
        string NewPath;
        AssemblyDefinition TerrariaServer;
        public Patcher(string path, string newPath)
        {
            this.NewPath = newPath;
            TerrariaServer = AssemblyDefinition.ReadAssembly(path);
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

        public void Patch()
        {
            PatchTitle();
            Save();
        }

        private void PatchTitle()
        {
            MethodDefinition DedServ = GetMethodDefinition(TerrariaServer, "Terraria.Main", "DedServ", 0);
            ILProcessor il = DedServ.Body.GetILProcessor();
            Instruction text = Instruction.Create(OpCodes.Ldstr, "Tewwawia"); //8 terraria
            Instruction text2 = Instruction.Create(OpCodes.Ldstr, "Tewwawia Server ");//Terraria
            il.Replace(DedServ.Body.Instructions[17], text);
            il.Replace(DedServ.Body.Instructions[85], text2);
        }
        
        private void Save()
        {
            TerrariaServer.Write(NewPath);
        }
    }
}
