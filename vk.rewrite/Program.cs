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
        static MethodReference s_stringToHGlobalUtf8Ref;
        static MethodDefinition s_freeHGlobalRef;
        static TypeReference s_stringHandleRef;

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
                s_stringHandleRef = mainModule.GetType (mainVkNamespace+".StringHandle");

                TypeDefinition bindingHelpers = mainModule.GetType(mainVkNamespace+".BindingsHelpers");

                s_stringToHGlobalUtf8Ref	= bindingHelpers.Methods.Single (md => md.Name == "StringToHGlobalUtf8");
                s_freeHGlobalRef			= bindingHelpers.Methods.Single (md => md.Name == "FreeHGlobal");

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

                if (parameterType.FullName == "System.String")
                {
                    VariableDefinition variableDef = new VariableDefinition(s_stringHandleRef);
                    method.Body.Variables.Add(variableDef);
                    il.Emit(OpCodes.Call, s_stringToHGlobalUtf8Ref);
                    il.Emit(OpCodes.Stloc, variableDef);
                    il.Emit(OpCodes.Ldloc, variableDef);
                    stringParams.Add(variableDef);
                }                
            }
				            
            FieldDefinition field = method.DeclaringType.Fields.SingleOrDefault(fd => fd.Name == method.Name + "_ptr");
            
			if (field == null)            
                throw new InvalidOperationException("Can't find function pointer field for " + method.Name);
            
            il.Emit(OpCodes.Ldsfld, field);

            CallSite callSite = new CallSite(method.ReturnType) {CallingConvention = MethodCallingConvention.StdCall};

			foreach (ParameterDefinition pd in method.Parameters)
            {

				if (pd.ParameterType.FullName == "System.String")
					callSite.Parameters.Add (new ParameterDefinition (pd.Name, pd.Attributes, s_stringHandleRef));
				else
					callSite.Parameters.Add (pd);
            }

            il.Emit(OpCodes.Calli, callSite);

            foreach (VariableDefinition stringVar in stringParams)
            {
                il.Emit(OpCodes.Ldloc, stringVar);
                il.Emit(OpCodes.Call, s_freeHGlobalRef);
            }

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