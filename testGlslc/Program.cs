using System;
using System.Diagnostics;
using System.IO;

namespace testGlslc {
	class Program {
		static void Main (string[] args) {

			string SourceFile = "/mnt/devel/gts/vk.net/samples/Triangle/shaders/triangle.frag";
			string DestinationFile = "/tmp/triangle.frag.spv";


			string glslcPath = Path.Combine (Environment.GetEnvironmentVariable ("VULKAN_SDK"), "bin");
			glslcPath = Path.Combine (glslcPath, "glslc");



			Process glslc = new Process ();
			glslc.StartInfo.StandardOutputEncoding = System.Text.Encoding.ASCII;
			glslc.StartInfo.StandardErrorEncoding = System.Text.Encoding.ASCII;
			glslc.StartInfo.UseShellExecute = false;
			glslc.StartInfo.RedirectStandardOutput = true;
			glslc.StartInfo.RedirectStandardError = true;
			glslc.StartInfo.FileName = glslcPath;
			glslc.StartInfo.Arguments = $"{SourceFile} -o {DestinationFile}";
			glslc.StartInfo.CreateNoWindow = true;

			glslc.EnableRaisingEvents = true;
			glslc.OutputDataReceived += Glslc_OutputDataReceived;
			glslc.ErrorDataReceived += Glslc_ErrorDataReceived;

			glslc.Start ();



			glslc.BeginErrorReadLine ();
			glslc.BeginOutputReadLine ();


			glslc.WaitForExit ();


		}

		static void Glslc_ErrorDataReceived (object sender, DataReceivedEventArgs e) {
			if (e.Data == null)
				return;
			if (string.Equals (e.Data, "(0)", StringComparison.Ordinal))
				return;
			string[] tmp = e.Data.Split (':');
			if (tmp.Length == 5) {
				int line = int.Parse (tmp[1]);
				Console.WriteLine ($"file: {tmp[0]} line:{line}");
			} else {
				tmp = e.Data.Split (' ');
				Console.WriteLine ($"error count = {int.Parse (tmp[0])}");
			}
			Console.WriteLine ($"=> {e.Data}");
		}


		static void Glslc_OutputDataReceived (object sender, DataReceivedEventArgs e) {
			if (e.Data == null)
				return;
			if (string.Equals (e.Data, "(0)", StringComparison.Ordinal))
				return;
			Console.WriteLine (e.Data);
		}
	}
}
