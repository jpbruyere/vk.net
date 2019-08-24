using System;
using System.Runtime.InteropServices;
using VK;
using static VK.Vk;

namespace CVKL.DebugUtils {

    public class Messenger : IDisposable {
		Instance inst;
		VkDebugUtilsMessengerEXT handle;
		PFN_vkDebugUtilsMessengerCallbackEXT onMessage = new PFN_vkDebugUtilsMessengerCallbackEXT(HandlePFN_vkDebugUtilsMessengerCallbackEXT);

		static VkBool32 HandlePFN_vkDebugUtilsMessengerCallbackEXT (VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlagsEXT messageTypes, IntPtr pCallbackData, IntPtr pUserData) {
			//Console.WriteLine ("{0} {1}: {2}",messageSeverity, messageTypes, Marshal.PtrToStringAnsi(pUserData));
			Console.WriteLine ("MESSAGE RECEIVED");
			return false;
		}



   //     PFN_vkDebugReportCallbackEXT debugCallbackDelegate = new PFN_vkDebugReportCallbackEXT (debugCallback);

   //     static VkBool32 debugCallback (VkDebugReportFlagsEXT flags, VkDebugReportObjectTypeEXT objectType, ulong obj,
   //         UIntPtr location, int messageCode, IntPtr pLayerPrefix, IntPtr pMessage, IntPtr pUserData) {
   //         string prefix = "";
   //         switch (flags) {
   //             case 0:
   //                 prefix = "?";
   //                 break;
   //             case VkDebugReportFlagsEXT.InformationEXT:
			//		Console.ForegroundColor = ConsoleColor.Gray;
			//		prefix = "INFO";
   //                 break;
   //             case VkDebugReportFlagsEXT.WarningEXT:
			//		Console.ForegroundColor = ConsoleColor.DarkYellow;
			//		prefix = "WARN";
   //                 break;
   //             case VkDebugReportFlagsEXT.PerformanceWarningEXT:
			//		Console.ForegroundColor = ConsoleColor.Yellow;
			//		prefix = "PERF";
   //                 break;
   //             case VkDebugReportFlagsEXT.ErrorEXT:
			//		Console.ForegroundColor = ConsoleColor.DarkRed;
			//		prefix = "EROR";
			//		break;
   //             case VkDebugReportFlagsEXT.DebugEXT:
			//		Console.ForegroundColor = ConsoleColor.Red;
			//		prefix = "DBUG";
   //                 break;
   //         }

   //         Console.WriteLine ("{0} {1}: {2}",prefix, messageCode, Marshal.PtrToStringAnsi(pMessage));
			//Console.ForegroundColor = ConsoleColor.White;
        //    return VkBool32.False;
        //}
        
        public Messenger (Instance instance, VkDebugReportFlagsEXT flags = VkDebugReportFlagsEXT.ErrorEXT | VkDebugReportFlagsEXT.WarningEXT) {
			inst = instance;
			VkDebugUtilsMessengerCreateInfoEXT info = VkDebugUtilsMessengerCreateInfoEXT.New ();
			info.messageType = VkDebugUtilsMessageTypeFlagsEXT.ValidationEXT | VkDebugUtilsMessageTypeFlagsEXT.GeneralEXT | VkDebugUtilsMessageTypeFlagsEXT.PerformanceEXT;
			info.messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.ErrorEXT | VkDebugUtilsMessageSeverityFlagsEXT.WarningEXT | VkDebugUtilsMessageSeverityFlagsEXT.InfoEXT | VkDebugUtilsMessageSeverityFlagsEXT.VerboseEXT;
			info.pfnUserCallback = Marshal.GetFunctionPointerForDelegate (onMessage);

			Utils.CheckResult (vkCreateDebugUtilsMessengerEXT (inst.VkInstance, ref info, IntPtr.Zero, out handle));
        }

		#region IDisposable Support
		private bool disposedValue;

		protected virtual void Dispose (bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					// TODO: supprimer l'état managé (objets managés).
				}

				vkDestroyDebugUtilsMessengerEXT (inst.Handle, handle, IntPtr.Zero);

				disposedValue = true;
			}
		}

		~Messenger () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion
	}
}
