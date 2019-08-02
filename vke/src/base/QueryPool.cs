//
// CommandPool.cs
//
// Author:
//       Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// Copyright (c) 2019 jp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using VK;
using static VK.Vk;

namespace CVKL {
	public class TimestampQueryPool : QueryPool {
		public readonly float Period;

		#region CTORS
		public TimestampQueryPool (Device device, uint count = 2)
		: base (device, VkQueryType.Timestamp, 0, count)
		{
			Period = Dev.phy.Limits.timestampPeriod;

			resultLength = 1;

			Activate ();
		}
		#endregion

		public void Write (CommandBuffer cmd, uint query, VkPipelineStageFlags stageFlags = VkPipelineStageFlags.BottomOfPipe) {
			vkCmdWriteTimestamp (cmd.Handle, stageFlags, handle, query);
		}
		public void Start (CommandBuffer cmd, VkPipelineStageFlags stageFlags = VkPipelineStageFlags.BottomOfPipe) {
			vkCmdWriteTimestamp (cmd.Handle, stageFlags, handle, 0);
		}
		public void End (CommandBuffer cmd, VkPipelineStageFlags stageFlags = VkPipelineStageFlags.BottomOfPipe) {
			vkCmdWriteTimestamp (cmd.Handle, stageFlags, handle, 1);
		}
		public float ElapsedMiliseconds {
			get {
				ulong[] res = GetResults ();
				return (res[1] - res[0]) * Period / 1000000f;
			}
		}
	}
	public class PipelineStatisticsQueryPool : QueryPool {

		public readonly VkQueryPipelineStatisticFlags[] RequestedStats;

		#region CTORS
		public PipelineStatisticsQueryPool (Device device, VkQueryPipelineStatisticFlags statisticFlags, uint count = 1)
		: base (device, VkQueryType.PipelineStatistics, statisticFlags, count)
		{
			List<VkQueryPipelineStatisticFlags> requests = new List<VkQueryPipelineStatisticFlags> ();

			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.InputAssemblyVertices))
				requests.Add (VkQueryPipelineStatisticFlags.InputAssemblyVertices);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.InputAssemblyPrimitives))
				requests.Add (VkQueryPipelineStatisticFlags.InputAssemblyPrimitives);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.VertexShaderInvocations))
				requests.Add (VkQueryPipelineStatisticFlags.VertexShaderInvocations);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.GeometryShaderInvocations))
				requests.Add (VkQueryPipelineStatisticFlags.GeometryShaderInvocations);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.GeometryShaderPrimitives))
				requests.Add (VkQueryPipelineStatisticFlags.GeometryShaderPrimitives);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.ClippingInvocations))
				requests.Add (VkQueryPipelineStatisticFlags.ClippingInvocations);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.ClippingPrimitives))
				requests.Add (VkQueryPipelineStatisticFlags.ClippingPrimitives);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.FragmentShaderInvocations))
				requests.Add (VkQueryPipelineStatisticFlags.FragmentShaderInvocations);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.TessellationControlShaderPatches))
				requests.Add (VkQueryPipelineStatisticFlags.TessellationControlShaderPatches);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.TessellationEvaluationShaderInvocations))
				requests.Add (VkQueryPipelineStatisticFlags.TessellationEvaluationShaderInvocations);
			if (statisticFlags.HasFlag (VkQueryPipelineStatisticFlags.ComputeShaderInvocations))
				requests.Add (VkQueryPipelineStatisticFlags.ComputeShaderInvocations);

			RequestedStats = requests.ToArray ();

			resultLength = (uint)requests.Count;

			Activate ();
		}
		#endregion

		public void Begin (CommandBuffer cmd, uint query = 0) {
			vkCmdBeginQuery (cmd.Handle, handle, query, VkQueryControlFlags.Precise);
		}
		public void End (CommandBuffer cmd, uint query = 0) {
			vkCmdEndQuery (cmd.Handle, handle, query);
		}
	}

	public abstract class QueryPool : Activable {        
        protected VkQueryPool handle;
		protected readonly VkQueryPoolCreateInfo createInfos;
		public readonly VkQueryType QueryType;
		protected uint resultLength;

		#region CTORS
		protected QueryPool (Device device, VkQueryType queryType, VkQueryPipelineStatisticFlags statisticFlags, uint count = 1)
		: base(device)
        {
			createInfos = VkQueryPoolCreateInfo.New (queryType, statisticFlags, count);

			//Activate ();
        }

		#endregion

		protected override VkDebugMarkerObjectNameInfoEXT DebugMarkerInfo
			=> new VkDebugMarkerObjectNameInfoEXT(VkDebugReportObjectTypeEXT.QueryPoolEXT, handle.Handle);

		public override void Activate () {
			if (state != ActivableState.Activated) {
				VkQueryPoolCreateInfo infos = createInfos;     	        
	            Utils.CheckResult (vkCreateQueryPool (Dev.VkDev, ref infos, IntPtr.Zero, out handle));
			}
			base.Activate ();
		}

		public ulong[] GetResults () {
			ulong[] results = new ulong[resultLength * createInfos.queryCount];
			IntPtr ptr = results.Pin ();
			vkGetQueryPoolResults (Dev.VkDev, handle, 0, createInfos.queryCount, (UIntPtr)(resultLength * createInfos.queryCount* sizeof (ulong)), ptr, sizeof (ulong), VkQueryResultFlags.QueryResult64);
			results.Unpin ();
			return results;
		}

		    
		public override string ToString () {
			return string.Format ($"{base.ToString ()}[0x{handle.Handle.ToString("x")}]");
		}

		#region IDisposable Support
		protected override void Dispose (bool disposing) {
			if (!disposing)
				System.Diagnostics.Debug.WriteLine ("VKE QueryPool disposed by finalizer");
			if (state == ActivableState.Activated)
				vkDestroyQueryPool (Dev.VkDev, handle, IntPtr.Zero);
			base.Dispose (disposing);
		}
		#endregion

	}
}
