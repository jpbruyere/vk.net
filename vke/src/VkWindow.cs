//
// VkWindow.cs
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
using System.Linq;
using System.Diagnostics;
using Glfw;
using VK;
using static VK.Vk;

namespace CVKL {
	/// <summary>
	/// Base class to build vulkan application.
	/// Provide default swapchain with its command pool and buffers per image and the main present queue
	/// </summary>
    public abstract class VkWindow : IDisposable {
		static VkWindow currentWindow;

		IntPtr hWin;

#if DEBUG
		DebugReport dbgRepport;
#endif

        protected VkSurfaceKHR hSurf;
        protected Instance instance;
        protected PhysicalDevice phy;
        protected Device dev;
        protected PresentQueue presentQueue;
        protected SwapChain swapChain;
        protected CommandPool cmdPool;
        protected CommandBuffer[] cmds;
        protected VkSemaphore[] drawComplete;

		protected uint fps;
		protected bool updateViewRequested = true, rebuildBuffers = false;
		protected double lastMouseX, lastMouseY;
		protected bool[] MouseButton => buttons;

		/// <summary>
		/// default camera
		/// </summary>
		protected Camera camera = new Camera (Utils.DegreesToRadians (45f), 1f);

        uint width, height;
		bool[] buttons = new bool[10];

		uint frameCount;
		Stopwatch frameChrono;

		/// <summary>
		/// Override this property to change the list of enabled extensions
		/// </summary>
		public virtual string[] EnabledExtensions {
			get {
				return new string[] {Ext.D.VK_KHR_swapchain};
			} 
		}
		/// <summary>
		/// Frequency in millisecond of the call to the Update method
		/// </summary>
		public long UpdateFrequency = 200;

        public uint Width => width;
        public uint Height => height;

        public VkWindow (bool debugMarkers = false, string name = "VkWindow", uint _width = 1024, uint _height=768, bool vSync = false)
        {
            currentWindow = this;

            width = _width;
            height = _height;

            Glfw3.Init ();

            Glfw3.WindowHint (WindowAttribute.ClientApi, 0);
            Glfw3.WindowHint (WindowAttribute.Resizable, 1);

			hWin = Glfw3.CreateWindow ((int)width, (int)height, name, MonitorHandle.Zero, IntPtr.Zero);

            Glfw3.SetKeyCallback (hWin, HandleKeyDelegate);
            Glfw3.SetMouseButtonPosCallback (hWin, HandleMouseButtonDelegate);
            Glfw3.SetCursorPosCallback (hWin, HandleCursorPosDelegate);
            Glfw3.SetWindowSizeCallback (hWin, HandleWindowSizeDelegate);

            initVulkan (vSync, debugMarkers);
        }

		void initVulkan (bool vSync, bool debugMarkers) {
            instance = new Instance ();

#if DEBUG
			dbgRepport = new DebugReport (instance,
				VkDebugReportFlagsEXT.ErrorEXT 
				| VkDebugReportFlagsEXT.DebugEXT 
				| VkDebugReportFlagsEXT.WarningEXT 
				| VkDebugReportFlagsEXT.PerformanceWarningEXT 
				//| VkDebugReportFlagsEXT.InformationEXT
			);
#endif

			hSurf = instance.CreateSurface (hWin);

            phy = instance.GetAvailablePhysicalDevice ().Where (p => p.HasSwapChainSupport).FirstOrDefault ();

            VkPhysicalDeviceFeatures enabledFeatures = default(VkPhysicalDeviceFeatures);
            configureEnabledFeatures (phy.Features, ref enabledFeatures);

			if (debugMarkers)
				debugMarkers = phy.GetDeviceExtensionSupported (Ext.D.VK_EXT_debug_marker);
			//First create the c# device class
			dev = new Device (phy, debugMarkers);
            //create queue class
            createQueues ();

            //activate the device to have effective queues created accordingly to what's available
            dev.Activate (enabledFeatures, EnabledExtensions);

            swapChain = new SwapChain (presentQueue as PresentQueue, width, height, VkFormat.B8g8r8a8Srgb,
                vSync ? VkPresentModeKHR.FifoKHR : VkPresentModeKHR.MailboxKHR );

            cmdPool = new CommandPool(dev, presentQueue.qFamIndex);

            cmds = new CommandBuffer[swapChain.ImageCount];
            drawComplete = new VkSemaphore[swapChain.ImageCount];

            for (int i = 0; i < swapChain.ImageCount; i++)
                drawComplete[i] = dev.CreateSemaphore ();

			cmdPool.SetName ("main CmdPool");
			for (int i = 0; i < swapChain.ImageCount; i++)
				drawComplete[i].SetDebugMarkerName (dev, "Semaphore DrawComplete" + i);
        }
		/// <summary>
		/// override this method to modify enabled features before device creation
		/// </summary>
		/// <param name="features">Features.</param>
        protected virtual void configureEnabledFeatures (VkPhysicalDeviceFeatures available_features, ref VkPhysicalDeviceFeatures features) {
        }
		/// <summary>
		/// override this method to create additional queue. Dedicated queue of the requested type will be selected first, created queues may excess
		/// available physical queues.
		/// </summary>
        protected virtual void createQueues () {
            presentQueue = new PresentQueue (dev, VkQueueFlags.Graphics, hSurf);
        }

		/// <summary>
		/// Main render method called each frame. get next swapchain image, process resize if needed, submit and present to the presentQueue.
		/// Wait QueueIdle after presenting.
		/// </summary>
        protected virtual void render () {
            int idx = swapChain.GetNextImage();
            if (idx < 0) {
                OnResize ();
                return;
            }

            presentQueue.Submit (cmds[idx], swapChain.presentComplete, drawComplete[idx]);
            presentQueue.Present (swapChain, drawComplete[idx]);

            presentQueue.WaitIdle ();
        }


        protected virtual void onMouseMove (double xPos, double yPos) { 
			double diffX = lastMouseX - xPos;
			double diffY = lastMouseY - yPos;
			if (MouseButton[0]) {
				camera.Rotate ((float)-diffX,(float)-diffY);
			} else if (MouseButton[1]) {
				camera.Move (0,0,(float)diffY);
			}

			updateViewRequested = true;
        }
        protected virtual void onMouseButtonDown (Glfw.MouseButton button) { }
        protected virtual void onMouseButtonUp (Glfw.MouseButton button) { }
		protected virtual void onKeyDown (Key key, int scanCode, Modifier modifiers) { 
			switch (key) {
				case Key.F4:
					if (modifiers == Modifier.Alt)
						Glfw3.SetWindowShouldClose (currentWindow.hWin, 1);
					break;
				case Key.Escape:
					Glfw3.SetWindowShouldClose (currentWindow.hWin, 1);
					break;
				case Key.Up:
					camera.Move (0, 0, 1);
					break;
				case Key.Down:
					camera.Move (0, 0, -1);
					break;
				case Key.Left:
					camera.Move (1, 0, 0);
					break;
				case Key.Right:
					camera.Move (-1, 0, 0);
					break;
				case Key.PageUp:
					camera.Move (0, 1, 0);
					break;
				case Key.PageDown:
					camera.Move (0, -1, 0);
					break;
				case Key.F3:
					if (camera.Type == Camera.CamType.FirstPerson)
						camera.Type = Camera.CamType.LookAt;
					else
						camera.Type = Camera.CamType.FirstPerson;
					break;
			}
			updateViewRequested = true;
		}
		protected virtual void onKeyUp (Key key, int scanCode, Modifier modifiers) { }

        static void HandleWindowSizeDelegate (IntPtr window, int width, int height) {}
        static void HandleCursorPosDelegate (IntPtr window, double xPosition, double yPosition) {
            currentWindow.onMouseMove (xPosition, yPosition);
			currentWindow.lastMouseX = xPosition;
			currentWindow.lastMouseY = yPosition;
        }
        static void HandleMouseButtonDelegate (IntPtr window, Glfw.MouseButton button, InputAction action, Modifier mods) {
            if (action == InputAction.Press) {
                currentWindow.buttons[(int)button] = true;
                currentWindow.onMouseButtonDown (button);
            } else {
                currentWindow.buttons[(int)button] = false;
                currentWindow.onMouseButtonUp (button);
            }
        }
        static void HandleKeyDelegate (IntPtr window, Key key, int scanCode, InputAction action, Modifier modifiers) {
			if (action == InputAction.Press || action == InputAction.Repeat) {
				currentWindow.onKeyDown (key, scanCode, modifiers);
			} else { 
				currentWindow.onKeyUp (key, scanCode, modifiers);
			}
        }
        
		/// <summary>
		/// main window loop, exits on GLFW3 exit event
		/// </summary>
        public virtual void Run () {
            OnResize ();
            UpdateView ();

            frameChrono = Stopwatch.StartNew ();
			long totTime = 0;

            while (!Glfw3.WindowShouldClose (hWin)) {
                render ();

                if(updateViewRequested)
                    UpdateView ();

                frameCount++;

                if (frameChrono.ElapsedMilliseconds > UpdateFrequency) {
					Update ();

					frameChrono.Stop ();
					totTime += frameChrono.ElapsedMilliseconds;
                    fps = (uint)((double)frameCount  / (double)totTime * 1000.0);
					//Glfw3.SetWindowTitle (hWin, "FPS: " + fps.ToString ());                    
					if (totTime > 2000) {
						frameCount = 0;
						totTime = 0;
					}
					frameChrono.Restart ();
                }
                Glfw3.PollEvents ();
            }
        }
        public virtual void UpdateView () {}
		/// <summary>
		/// custom update method called at UpdateFrequency
		/// </summary>
		public virtual void Update () {}

		/// <summary>
		/// called when swapchain has been resized, override this method to resize your framebuffers coupled to the swapchain
		/// </summary>
		protected virtual void OnResize () {}


		#region IDisposable Support
		protected bool isDisposed;

		protected virtual void Dispose (bool disposing) {
			if (!isDisposed) {
				dev.WaitIdle ();

				for (int i = 0; i < swapChain.ImageCount; i++) {
					dev.DestroySemaphore (drawComplete[i]);
					cmds[i].Free ();
				}

				swapChain.Destroy ();

				vkDestroySurfaceKHR (instance.Handle, hSurf, IntPtr.Zero);

				cmdPool.Dispose ();

				if (disposing) {
					dev.Dispose ();
#if DEBUG
					dbgRepport.Dispose ();
#endif
					instance.Dispose ();
				} else
					Debug.WriteLine ("a VkWindow has not been correctly disposed");

				Glfw3.DestroyWindow (hWin);
				Glfw3.Terminate ();


				isDisposed = true;
			}
		}

		~VkWindow () {
			Dispose (false);
		}
		public void Dispose () {
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		#endregion
	}
}
