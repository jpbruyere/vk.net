using System.Linq;
using System.Runtime.InteropServices;

namespace Vk.Generator
{
    public static class CommandHelpers
    {
        public static void WriteCommand(CsCodeWriter cw, TypeNameMappings tnm, CommandDefinition command)
        {
            if (!command.IsVariant)
            {
                cw.WriteLine($"private static IntPtr {command.Name}_ptr;");
                //try to cancel unsafe pointer if variant exist
                string[] noskip = { "vkEndCommandBuffer", "vkDeviceWaitIdle", "vkBindBufferMemory", "vkMapMemory",
                "vkUnmapMemory", "vkBindImageMemory", "vkResetDescriptorPool", "vkCmdEndRenderPass", "vkCmdBindPipeline",
                "vkCmdBindIndexBuffer", "vkCmdDrawIndexed" };
                if (!noskip.Contains(command.Name))
                    return;
                //if (command.Parameters.Any (p => p.Type.PointerIndirection > 0))
            }
            string[] skip = { "vkGetPhysicalDeviceMemoryProperties" };
            if (skip.Contains (command.Name))
                return;

            if (command.SuccessCodes.Length != 0)
            {
                cw.WriteLine($"///<remarks>Success codes:{string.Join(", ", command.SuccessCodes)}. Error codes:{string.Join(", ", command.ErrorCodes)}</remarks>");
            }

            cw.WriteLine("[Generator.CalliRewrite]");
            using (cw.PushBlock($"public static unsafe {command.ReturnType.MapTypeSpec(tnm)} {command.Name}({command.GetParametersSignature(tnm)})"))
            {
                cw.WriteLine("throw new NotImplementedException();");
            }
        }
    }
}
