namespace Glfw
{
    /// <summary>
    /// Attributes of a window or its framebuffer or context, that can be
    /// configured/queried at runtime or hinted at creation.
    /// </summary>
    public enum WindowAttribute
    {
        /// <summary>
        /// Whether the specified window has input focus. This hint is ignored
        /// for full screen and initially hidden windows.
        /// </summary>
        Focused = 0x00020001,
        /// <summary>
        /// Whether the specified window is iconified (minimized).
        /// </summary>
        Iconified = 0x00020002,
        /// <summary>
        /// Whether the specified window is resizable by the user. This is set
        /// on creation.
        /// </summary>
        Resizable = 0x00020003,
        /// <summary>
        /// Whether the specified window is visible.
        /// </summary>
        Visible = 0x00020004,
        /// <summary>
        /// Whether the specified window has decorations such as a border, a
        /// close widget, etc. This is set on creation.
        /// </summary>
        Decorated = 0x00020005,
        /// <summary>
        /// Whether the full screen window will automatically iconify and
        /// restore the previous default framebuffer on input focus loss. This hint is
        /// ignored for windowed mode windows.
        /// </summary>
        AutoIconify = 0x00020006,
        /// <summary>
        /// Whether the windowed mode window will be floating above other
        /// regular windows, also called topmost or always-on-top. This is
        /// intended primarily for debugging purposes and cannot be used to
        /// implement proper full screen windows. This hint is ignored for full
        /// screen windows.
        /// </summary>
        Floating = 0x00020007,
        /// <summary>
        /// Whether the specified window is maximized.
        /// </summary>
        Maximized = 0x00020008,
        /// <summary>
        /// The bit depth of the red channel of the default framebuffer.
        /// </summary>
        RedBits = 0x00021001,
        /// <summary>
        /// The bit depth of the green channel of the default framebuffer.
        /// </summary>
        GreenBits = 0x00021002,
        /// <summary>
        /// The bit depth of the blue channel of the default framebuffer.
        /// </summary>
        BlueBits = 0x00021003,
        /// <summary>
        /// The bit depth of the alpha channel of the default framebuffer.
        /// </summary>
        AlphaBits = 0x00021004,
        /// <summary>
        /// The bit depth of the depth channel of the default framebuffer.
        /// </summary>
        DepthBits = 0x00021005,
        /// <summary>
        /// The bit depth of the stencil channel of the default framebuffer.
        /// </summary>
        StencilBits = 0x00021006,
        /// <summary>
        /// The desired bit depth of the red channel of the accumulation
        /// buffer. Accumulation buffers are a legacy OpenGL feature and
        /// should not be used in new code.
        /// </summary>
        AccumRedBits = 0x00021007,
        /// <summary>
        /// The desired bit depth of the green channel of the accumulation
        /// buffer. Accumulation buffers are a legacy OpenGL feature and
        /// should not be used in new code.
        /// </summary>
        AccumGreenBits = 0x00021008,
        /// <summary>
        /// The desired bit depth of the blue channel of the accumulation
        /// buffer. Accumulation buffers are a legacy OpenGL feature and
        /// should not be used in new code.
        /// </summary>
        AccumBlueBits = 0x00021009,
        /// <summary>
        /// The desired bit depth of the alpha channel of the accumulation
        /// buffer. Accumulation buffers are a legacy OpenGL feature and
        /// should not be used in new code.
        /// </summary>
        AccumAlphaBits = 0x0002100A,
        /// <summary>
        ///  The desired number of auxiliary buffers. Auxiliary buffers are a
        ///  legacy OpenGL feature and should not be used in new code.
        /// </summary>
        AuxBuffers = 0x0002100B,
        /// <summary>
        /// Whether to use stereoscopic rendering. 
        /// </summary>
        Stereo = 0x0002100C,
        /// <summary>
        /// The desired number of samples to use for multisampling. Zero
        /// disables multisampling.
        /// </summary>
        Samples = 0x0002100D,
        /// <summary>
        /// Whether the framebuffer should be sRGB capable.
        /// </summary>
        SrgbCapable = 0x0002100E,
        /// <summary>
        /// The desired refresh rate for full screen windows. This hint is
        /// ignored for windowed mode windows.
        /// </summary>
        RefreshRate = 0x0002100F,
        /// <summary>
        /// Whether the framebuffer should be double buffered.
        /// </summary>
        DoubleBuffer = 0x00021010,
        /// <summary>
        /// Specifies which client API to create the context for.
        /// </summary>
        ClientApi = 0x00022001,
        /// <summary>
        /// Specifies the major component of the client API version of the
        /// window's context.
        /// </summary>
        ContextVersionMajor = 0x00022002,
        /// <summary>
        /// Specifies the minor component of the client API version of the
        /// window's context.
        /// </summary>
        ContextVersionMinor = 0x00022003,
        /// <summary>
        /// Specifies the revision component of the client API version of the
        /// window's context.
        /// </summary>
        ContextRevision = 0x00022004,
        /// <summary>
        /// The robustness strategy used by the context.
        /// </summary>
        ContextRobustness = 0x00022005,
        /// <summary>
        /// Specifies whether the OpenGL context should be forward-compatible,
        /// i.e. one where all functionality deprecated in the requested
        /// version of OpenGL is removed. If OpenGL ES is requested, this hint
        /// is ignored.
        /// </summary>
        OpenGlForwardCompat = 0x00022006,
        /// <summary>
        /// Specifies whether to create a debug OpenGL context, which may have
        /// additional error and performance issue reporting functionality. If
        /// OpenGL ES is requested, this hint is ignored.
        /// </summary>
        OpenGlDebugContext = 0x00022007,
        /// <summary>
        /// The OpenGL profile used by the context.
        /// </summary>
        OpenGlProfile = 0x00022008,
        /// <summary>
        /// Specifies the release behavior to be used by the context.
        /// </summary>
        ContextReleaseBehavior = 0x00022009,
        /// <summary>
        /// Specifies whether errors should be generated by the context. If
        /// enabled, situations that would have generated errors instead cause
        /// undefined behavior.
        /// </summary>
        ContextNoError = 0x0002200A,
        /// <summary>
        /// The context creation API used to create the window's context.
        /// </summary>
        ContextCreationApi = 0x0002200B
    }
}
