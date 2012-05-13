using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.Net.Sockets;

namespace ExtractIL
{
	class Program
	{
		class Netplay
		{
			public class serversock
			{
				public TcpClient tcpClient;
				public string name;
			}
			public static serversock[] serverSock = new serversock[1];
		}
		class Plugin
		{
			public static void ThrowEvent(int id, params object[] args)
			{

			}
		}
		int k = 1;
		void damt()
		{
			Plugin.ThrowEvent(1, Netplay.serverSock[k].name, Netplay.serverSock[k].tcpClient.Client.RemoteEndPoint);
		}

		void b(string player, string a)
		{

		}

		static void Main(string[] args)
		{
			//Console.Write("File: ");
			string Path = "C:\\Users\\WildN00b\\Documents\\Visual Studio 2010\\Projects\\Tewwawia\\ExtractIL\\bin\\debug\\ExtractIL.exe";//Tewwawia\\lib\\TerrariaServer.exe";//Console.ReadLine();
			AssemblyDefinition prog = AssemblyDefinition.ReadAssembly(Path);
			Console.Write("Class: ");
			string Class = Console.ReadLine();
			Console.Write("Name: ");
			string Name = Console.ReadLine();

			StringBuilder b = new StringBuilder();
			MethodDefinition a = GetMethodDefinition(prog, Class, Name);
			for (int i = 0; i < a.Body.Instructions.Count; i++)
			{
				b.AppendLine(i + ": " + a.Body.Instructions[i].ToString());
			}
			System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\output.txt", b.ToString());
		}

		static MethodDefinition GetMethodDefinition(AssemblyDefinition file, string Class, string name)
		{
			TypeDefinition tdef = file.MainModule.GetType(Class);
			foreach (MethodDefinition mdef in tdef.Methods)
			{
				if (mdef.Name == name) return mdef;
			}
			throw new ArgumentException("Unable to find this method!");
		}
	}
}
