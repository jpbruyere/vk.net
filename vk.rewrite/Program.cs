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

		static TypeReference trefCalliRewrite;
		static TypeReference trefPin;
#if AUTO_SET_STYPE
		static TypeReference trefStructureType;
#endif
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

				trefCalliRewrite = mainModule.GetType (mainVkNamespace+".Generator.CalliRewriteAttribute");
				trefPin = mainModule.GetType (mainVkNamespace+".Generator.PinAttribute");
#if AUTO_SET_STYPE
				trefStructureType = mainModule.GetType (mainVkNamespace+".StructureTypeAttribute");
#endif
				foreach (TypeDefinition type in mainModule.Types) {
					foreach (MethodDefinition method in type.Methods) {
						if (method.CustomAttributes.Any(ca => ca.AttributeType == trefCalliRewrite))
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

			for (int i = 0; i < method.Parameters.Count; i++)
			{
				EmitLoadArgument (il, i, method.Parameters);
				TypeReference parameterType = method.Parameters[i].ParameterType;
				TypeDefinition td = parameterType.IsByReference ?
					parameterType.GetElementType() as TypeDefinition :
					parameterType as TypeDefinition;
#if AUTO_SET_STYPE
				PropertyDefinition pdSType = td?.Properties.FirstOrDefault (f=>f.Name == "sType");
				CustomAttribute ca = td == null ? null : td.CustomAttributes.FirstOrDefault (ca => ca.AttributeType == trefStructureType);
				int structType = 0;
				if (ca != null) {
					PropertyDefinition pd = (ca.AttributeType as TypeDefinition).Properties[0];
					structType = (int)ca.ConstructorArguments[0].Value;
				}
#endif

				if (method.Parameters[i].CustomAttributes.Any(ca => ca.AttributeType == trefPin)) {
					VariableDefinition variable = new VariableDefinition(parameterType);
					method.Body.Variables.Add(variable);
					il.Emit(OpCodes.Stloc, variable);
#if AUTO_SET_STYPE
					if (ca != null) {
						il.Emit(OpCodes.Ldloca, variable);
						il.Emit(OpCodes.Ldc_I4, structType);
						il.Emit(OpCodes.Call, pdSType.SetMethod);
					}
#endif
					il.Emit(OpCodes.Ldloca, variable);
					variable = new VariableDefinition(new PinnedType(new ByReferenceType(parameterType)));
					method.Body.Variables.Add(variable);
					il.Emit(OpCodes.Stloc, variable);
					il.Emit(OpCodes.Ldloc, variable);
					il.Emit(OpCodes.Conv_I);
				} else if (parameterType.IsByReference) {
					VariableDefinition byRefVariable = new VariableDefinition(new PinnedType(parameterType));
					method.Body.Variables.Add(byRefVariable);
					il.Emit(OpCodes.Stloc, byRefVariable);
#if AUTO_SET_STYPE
					if (pdSType != null) {
						il.Emit(OpCodes.Ldloc, byRefVariable);
						il.Emit(OpCodes.Ldc_I4, structType);
						il.Emit(OpCodes.Call, pdSType.SetMethod);
					}
#endif
					il.Emit(OpCodes.Ldloc, byRefVariable);
					il.Emit(OpCodes.Conv_I);
				}
#if AUTO_SET_STYPE
				else if (ca != null) {
					VariableDefinition variable = new VariableDefinition(parameterType);
					method.Body.Variables.Add(variable);
					il.Emit(OpCodes.Stloc, variable);
					il.Emit(OpCodes.Ldloca, variable);
					il.Emit(OpCodes.Ldc_I4, structType);
					il.Emit(OpCodes.Call, pdSType.SetMethod);
				}
#endif
			}

			FieldDefinition field = method.DeclaringType.Fields.SingleOrDefault(fd => fd.Name == method.Name + "_ptr");

			if (field == null)
				throw new InvalidOperationException("Can't find function pointer field for " + method.Name);

			il.Emit(OpCodes.Ldsfld, field);

			CallSite callSite = new CallSite(method.ReturnType) {CallingConvention = MethodCallingConvention.StdCall};

			foreach (ParameterDefinition pd in method.Parameters)
			{
				TypeReference parameterType;

				if (pd.ParameterType.IsByReference || pd.CustomAttributes.Any(ca => ca.AttributeType == trefPin))
					parameterType = new PointerType(pd.ParameterType.GetElementType());
				else
					parameterType = pd.ParameterType;

				ParameterDefinition calliPD = new ParameterDefinition(pd.Name, pd.Attributes, parameterType);

				callSite.Parameters.Add(calliPD);
			}
			il.Emit(OpCodes.Calli, callSite);

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