// Copyright (c) 2017 Eric Mellino
// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using Mono.Collections.Generic;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Vk.Rewrite
{
	public class Program
	{
		const string mainVkNamespace = "Vulkan";

		static TypeReference s_calliRewriteRef;

		public static int Main(string[] args)
		{


			if ((args.Length < 1) == true || args[0] == null) {
				Console.WriteLine ("Error: argument 1 must be the path of VK.dll.");
				return -1;
			}
			string vkDllPath = args[0];
			string outputPath = null;
			bool rewriteInPlace = false;

			if (outputPath == null)
			{
				outputPath = Path.GetTempFileName ();
				rewriteInPlace = true;
			}

			Rewrite(vkDllPath, outputPath);

			if (rewriteInPlace) {
				File.Delete (vkDllPath);
				File.Copy (outputPath, vkDllPath);
				File.Delete (outputPath);
				outputPath = vkDllPath;
			}

			Console.WriteLine (outputPath + " rewrited.");
			return 0;
		}

		private static void Rewrite(string vkDllPath, string outputPath)
		{
			using (AssemblyDefinition vkDll = AssemblyDefinition.ReadAssembly (vkDllPath))
			{
				ModuleDefinition mainModule = vkDll.Modules[0];

				s_calliRewriteRef = mainModule.GetType (mainVkNamespace+".Generator.CalliRewriteAttribute");

				foreach (TypeDefinition type in mainModule.Types)
				{

					foreach (MethodDefinition method in type.Methods)
					{
						if (method.CustomAttributes.Any(ca => ca.AttributeType == s_calliRewriteRef))
							RewriteMethod (method);
					}
				}
				vkDll.Write(outputPath);
			}
		}

		static void RewriteMethod (MethodDefinition method)
		{
			ILProcessor il = method.Body.GetILProcessor();
			il.Body.Instructions.Clear();

			List<VariableDefinition> stringParams = new List<VariableDefinition>();
			for (int i = 0; i < method.Parameters.Count; i++)
			{
				EmitLoadArgument (il, i, method.Parameters);
				TypeReference parameterType = method.Parameters[i].ParameterType;

				if (parameterType.IsGenericInstance) {
					if (parameterType.Name == "Nullable`1")
						System.Diagnostics.Debugger.Break ();
				}

				if (parameterType.IsByReference)
				{
					VariableDefinition byRefVariable = new VariableDefinition(new PinnedType(parameterType));
					method.Body.Variables.Add(byRefVariable);
					il.Emit(OpCodes.Stloc, byRefVariable);
					il.Emit(OpCodes.Ldloc, byRefVariable);
					il.Emit(OpCodes.Conv_I);
				}
			}

			FieldDefinition field = method.DeclaringType.Fields.SingleOrDefault(fd => fd.Name == method.Name + "_ptr");

			if (field == null)
				throw new InvalidOperationException("Can't find function pointer field for " + method.Name);

			il.Emit(OpCodes.Ldsfld, field);

			CallSite callSite = new CallSite(method.ReturnType) {CallingConvention = MethodCallingConvention.StdCall};

			foreach (ParameterDefinition pd in method.Parameters)
			{
				TypeReference parameterType;

				if (pd.ParameterType.IsByReference)
					parameterType = new PointerType(pd.ParameterType.GetElementType());
				else
					parameterType = pd.ParameterType;

				ParameterDefinition calliPD = new ParameterDefinition(pd.Name, pd.Attributes, parameterType);

				callSite.Parameters.Add(calliPD);
			}
			il.Emit(OpCodes.Calli, callSite);

			/*foreach (VariableDefinition stringVar in stringParams)
			{
				il.Emit(OpCodes.Ldloc, stringVar);
				il.Emit(OpCodes.Call, s_freeHGlobalRef);
			}*/

			il.Emit(OpCodes.Ret);

			if (method.Body.Variables.Count > 0)
				method.Body.InitLocals = true;
		}

		static void EmitLoadArgument(ILProcessor il, int i, Collection<ParameterDefinition> parameters)
		{
			switch (i) {
				case 0:
					il.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					il.Emit(OpCodes.Ldarg_1);
					break;
				case 2:
					il.Emit(OpCodes.Ldarg_2);
					break;
				case 3:
					il.Emit(OpCodes.Ldarg_3);
					break;
				default:
					il.Emit(OpCodes.Ldarg, i);
					break;
			}
		}
	}
}