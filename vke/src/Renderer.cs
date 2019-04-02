//
// Model.cs
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
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using glTFLoader;
using GL = glTFLoader.Schema;

using Vulkan;
using System.Collections.Generic;
using System.IO;



namespace VKE {
    using static Utils;
    	

	public class Renderer : IDisposable {
        Device dev;

        Image environmentCube;
        Image empty;
        Image lutBrdf;
        Image irradianceCube;
        Image prefilteredCube;
        Model scene;
        Model skybox;

        HostBuffer uboScene;
        HostBuffer uboSkybox;
        HostBuffer uboParams;

        enum PBRWorkflows { PBR_WORKFLOW_METALLIC_ROUGHNESS = 0, PBR_WORKFLOW_SPECULAR_GLOSINESS = 1 };

        struct Matrices {
            public Matrix4x4 projection;
            public Matrix4x4 model;
            public Matrix4x4 view;
            public Vector3 camPos;
        }

        struct LightParams {
            public Vector4 lightDir;
            public float exposure;
            public float gamma;
            public float prefilteredCubeMipLevels;
            public float scaleIBLAmbient;
            public float debugViewInputs;
            public float debugViewEquation;
        }
        struct LightSource {
            public Vector3 color;
            public Vector3 rotation;
        }
        [StructLayout (LayoutKind.Sequential)]
        struct Material {
            Vector4 baseColorFactor;
            Vector4 emissiveFactor;
            Vector4 diffuseFactor;
            Vector4 specularFactor;
            float workflow;
            int colorTextureSet;
            int PhysicalDescriptorTextureSet;
            int normalTextureSet;
            int occlusionTextureSet;
            int emissiveTextureSet;
            float metallicFactor;
            float roughnessFactor;
            float alphaMask;
            float alphaMaskCutoff;
        }

        LightSource lightSource = new LightSource {
            color = new Vector3 (1.0f),
            rotation = new Vector3 (75.0f, 40.0f, 0.0f),
        };
        LightParams shaderParams = new LightParams {
            exposure = 4.5f,
            gamma = 2.2f,
            scaleIBLAmbient = 1.0f,
        };
        Matrices sceneMats, skyboxMats;

        PipelineLayout pipelineLayout;
        Pipeline plPBR, plSkybox, plAlphaBlend;


        public Renderer (Device device) {
            dev = device; 
        }


		#region IDisposable Support
		private bool isDisposed = false; // Pour détecter les appels redondants

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				if (disposing) {
				} else
					Debug.WriteLine ("renderer was not disposed");
				isDisposed = true;
			}
		}
		public void Dispose () {
			Dispose (true);
		}
		#endregion
	}
}
