// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
namespace Glfw
{
    /// <summary>
    /// Indicates the general category of an error.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// GLFW has not been initialized.
        /// </summary>
        NotInitialised = 0x00010001,
        /// <summary>
        /// No context is current for this thread.
        /// </summary>
        NoCurrentContext,
        /// <summary>
        /// One of the arguments to the function was an invalid enum value.
        /// </summary>
        InvalidEnum,
        /// <summary>
        /// One of the arguments to the function was an invalid value.
        /// </summary>
        InvalidValue,
        /// <summary>
        /// A memory allocation failed.
        /// </summary>
        OutOfMemory,
        /// <summary>
        /// GLFW could not find support for the requested API on the system.
        /// </summary>
        ApiUnavailable,
        /// <summary>
        /// The requested OpenGL or OpenGL ES version is not available.
        /// </summary>
        VersionUnavailable,
        /// <summary>
        /// A platform-specific error occurred that does not match any of the more specific categories.
        /// </summary>
        PlatformError,
        /// <summary>
        /// The requested format is not supported or available.
        /// </summary>
        FormatUnavailable,
        /// <summary>
        /// The specified window does not have an OpenGL or OpenGL ES context.
        /// </summary>
        NoWindowContext
    }
}
