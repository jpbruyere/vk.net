using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;

namespace SpirVTasks {

	public class IncludeFileNotFound : FileNotFoundException {
		public string SourceFile;
		public int SourceLine;

		public IncludeFileNotFound(string srcFileName, int srcLine, string includeFileName) :
			base ("include file not found", includeFileName) {

			SourceFile = srcFileName;
			SourceLine = srcLine;
		}
	}

	public class CompileGLSLTask : Microsoft.Build.Utilities.Task {

		[Required]
		public ITaskItem SourceFile {
			get;
			set;
		}
		[Required]
		public ITaskItem TempDirectory {
			get;
			set;
		}
		public ITaskItem AdditionalIncludeDirectories {
			get;
			set;
		}
		[Required]
		[Output]
		public ITaskItem DestinationFile {
			get;
			set;
		}

		volatile bool success;

		bool tryFindInclude (string include, out string incFile) {
			if (!string.IsNullOrEmpty (AdditionalIncludeDirectories.ItemSpec)) {
				foreach (string incDir in AdditionalIncludeDirectories.ItemSpec.Split (';', ',', '|')) {
					incFile = Path.Combine (incDir, include);
					if (File.Exists (incFile))
						return true;
				}
			}
			incFile = "";
			return false;
		}

		void build_source (string src, StreamWriter temp) {
			using (StreamReader sr = new StreamReader (File.OpenRead (src))) {
				int srcLine = 0;
				while (!sr.EndOfStream) {
					string line = sr.ReadLine ();
					if (line.Trim ().StartsWith ("#include", StringComparison.Ordinal)) {
						string include = line.Split ('"', '<', '>')[1];
						string incFile = Path.Combine (Path.GetDirectoryName (src), include);

						if (!File.Exists (incFile)) {
							if (!tryFindInclude(include, out incFile))
								throw new IncludeFileNotFound (src, srcLine, include);
						}
						build_source (incFile, temp);
					} else
						temp.WriteLine (line);
					srcLine++;
				}
			}
		}


		public override bool Execute () {

			success = true;

			string glslcPath = Path.Combine (Environment.GetEnvironmentVariable ("VULKAN_SDK"), "bin");
			glslcPath = Path.Combine (glslcPath, "glslc");

			if (!File.Exists (glslcPath)) {
				BuildErrorEventArgs err = new BuildErrorEventArgs ("execute", "VK001", BuildEngine.ProjectFileOfTaskNode, 0, 0, 0, 0, $"glslc command not found: {glslcPath}", "Set 'VULKAN_SDK' environment variable", "SpirVTasks");
				BuildEngine.LogErrorEvent (err);
				return false;
			}

			//foreach (string item in SourceFile.MetadataNames) {
			//	Log.LogMessage (MessageImportance.High, $"METADATA name: {item}");
			//}

			string tempFile = Path.Combine (TempDirectory.ItemSpec, SourceFile.ItemSpec);
			if (File.Exists (tempFile))
				File.Delete (tempFile);
			try {
				Directory.CreateDirectory (Path.GetDirectoryName (tempFile));
				using (StreamWriter sw = new StreamWriter (File.OpenWrite(tempFile))) {
					string src = SourceFile.ItemSpec;
					build_source (SourceFile.ItemSpec, sw);
				}
			} catch (IncludeFileNotFound ex) {
				BuildErrorEventArgs err = new BuildErrorEventArgs ("include", "ERR_INC", ex.SourceFile, ex.SourceLine, 0, 0, 0, $"include file not found: {ex.FileName}", "", "SpirVTasks");
				BuildEngine.LogErrorEvent (err);
				return false;
			}catch (Exception ex) {
				BuildErrorEventArgs err = new BuildErrorEventArgs ("include", "ERR", SourceFile.ItemSpec, 0, 0, 0, 0, ex.ToString(), "", "SpirVTasks");
				BuildEngine.LogErrorEvent (err);
				return false;
			}

			Directory.CreateDirectory (Path.GetDirectoryName (DestinationFile.ItemSpec));

			Process glslc = new Process();
			glslc.StartInfo.StandardOutputEncoding = System.Text.Encoding.ASCII;
			glslc.StartInfo.StandardErrorEncoding = System.Text.Encoding.ASCII;
			glslc.StartInfo.UseShellExecute = false;
			glslc.StartInfo.RedirectStandardOutput = true;
			glslc.StartInfo.RedirectStandardError = true;
			glslc.StartInfo.FileName = glslcPath;
			glslc.StartInfo.Arguments = $"{tempFile} -o {DestinationFile.ItemSpec}";
			glslc.StartInfo.CreateNoWindow = true;

			glslc.EnableRaisingEvents = true;
			glslc.OutputDataReceived += Glslc_OutputDataReceived;
			glslc.ErrorDataReceived += Glslc_ErrorDataReceived;

			glslc.Start ();

			glslc.BeginErrorReadLine ();
			glslc.BeginOutputReadLine ();

			Log.LogMessage (MessageImportance.High, $"{SourceFile.ItemSpec} -> {DestinationFile.ItemSpec}");

			glslc.WaitForExit ();

			return success;
		}

		void Glslc_ErrorDataReceived (object sender, DataReceivedEventArgs e) {
			if (e.Data == null)
				return;

			if (string.Equals (e.Data, "(0)", StringComparison.Ordinal))
				return;

			string[] tmp = e.Data.Split (':');


			Log.LogMessage (MessageImportance.High, $"glslc: {e.Data}");

			if (tmp.Length == 5) {
				string srcFile = BuildEngine.ProjectFileOfTaskNode;
				int line = int.Parse (tmp[1]);

				BuildErrorEventArgs err = new BuildErrorEventArgs ("compile", tmp[2], srcFile, line, 0, 0, 0, $"{tmp[3]} {tmp[4]}", "no help", "SpirVTasks");
				BuildEngine.LogErrorEvent (err);
				success = false;
			} else {
				Log.LogMessage (MessageImportance.High, $"{e.Data}");
				//BuildErrorEventArgs err = new BuildErrorEventArgs ("unkn", "ERR0", BuildEngine.ProjectFileOfTaskNode, 0, 0, 0, 0, $"{e.Data}", "no help", "SpirVTasks");
				//BuildEngine.LogErrorEvent (err);
			}
		}


		void Glslc_OutputDataReceived (object sender, DataReceivedEventArgs e) {
			if (e.Data == null)
				return;
			if (string.Equals (e.Data, "(0)", StringComparison.Ordinal))
				return;

			Log.LogMessage (MessageImportance.High, $"data:{e.Data}");

			BuildWarningEventArgs taskEvent = new BuildWarningEventArgs ("glslc", "0", BuildEngine.ProjectFileOfTaskNode, 0, 0, 0, 0, $"{e.Data}", "no help", "SpirVTasks");
			BuildEngine.LogWarningEvent (taskEvent);
		}

	}
}
