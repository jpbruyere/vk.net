// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
namespace Glfw
{
    /// <summary>
    /// Events that may be raised from a monitor callback.
    /// </summary>
    public enum  MonitorEvent
    {
        /// <summary>
        /// The monitor was connected.
        /// </summary>
        Connected = 0x00040001,
        /// <summary>
        /// The monitor was disconnected.
        /// </summary>
        Disconnected = 0x00040002
    }
}
