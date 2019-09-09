// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)using System;
using System;
using System.Runtime.InteropServices;

namespace Glfw {
    /// <summary>
    /// The function signature for keyboard key callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that received the event.
    /// </param>
    /// <param name="key">
    /// The keyboard key that was pressed or released.
    /// </param>
    /// <param name="scanCode">
    /// The system-specific scancode of the key.
    /// </param>
    /// <param name="action">
    /// The input action that occured.
    /// </param>
    /// <param name="modifiers">
    /// Bit field describing which modifier keys were held down.
    /// </param>
    public delegate void KeyDelegate (IntPtr window, Key key, int scanCode, InputAction action, Modifier modifiers);
    /// <summary>
    /// A delegate representing character events on a WindowHandle.
    /// </summary>
    /// <param name="window">
    /// The window raising the event.
    /// </param>
    /// <param name="codepoint">
    /// The Unicode codepoint of the character.
    /// </param>
    public delegate void CharDelegate (IntPtr window, CodePoint codepoint);
    /// <summary>
    /// A delegate representing character events with modifiers on a WindowHandle.
    /// </summary>
    /// <param name="window">
    /// The window raising the event.
    /// </param>
    /// <param name="codepoint">
    /// The Unicode codepoint of the character.
    /// </param>
    /// <param name="modifiers">
    /// The modifiers applied to the character.
    /// </param>
    public delegate void CharModsDelegate (IntPtr window, CodePoint codepoint, Modifier modifiers);
    /// <summary>
    /// The function signature for cursor position callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that received the event.
    /// </param>
    /// <param name="xPosition">
    /// The new cursor x-coordinate, relative to the left edge of the client area.
    /// </param>
    /// <param name="yPosition">
    /// The new cursor y-coordinate, relative to the top edge of the client area.
    /// </param>
    public delegate void CursorPosDelegate (IntPtr window, double xPosition, double yPosition);
    /// <summary>
    /// The function signature for error callbacks.
    /// </summary>
    /// <param name="error">An error code giving the general category of the error.</param>
    /// <param name="description">A string description of the error.</param>
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void ErrorDelegate (ErrorCode error, [MarshalAs (UnmanagedType.LPStr)] string description);
    /// <summary>
    /// The function signature for monitor configuration callback functions.
    /// </summary>
    /// <param name="monitor">
    /// The monitor that was connected or disconnected.
    /// </param>
    /// <param name="eventStatus">
    /// The event that was raised.
    /// </param>
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void MonitorEventDelegate (MonitorHandle monitor, MonitorEvent eventStatus);
    /// <summary>
    /// The function signature for mouse button callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that received the event.
    /// </param>
    /// <param name="button">
    /// The mouse button that was pressed or released.
    /// </param>
    /// <param name="action">
    /// One of <see cref="InputAction.Press"/> or <see cref="InputAction.Release"/>.
    /// </param>
    /// <param name="mods">
    /// Bit field describing which modifier keys were held down.
    /// </param>
    public delegate void MouseButtonDelegate (IntPtr window, MouseButton button, InputAction action, Modifier mods);
    /// <summary>
    /// The function signature for scroll callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that received the event.
    /// </param>
    /// <param name="xOffset">
    /// The scroll offset along the x-axis.
    /// </param>
    /// <param name="yOffset">
    /// The scroll offset along the y-axis.
    /// </param>
    public delegate void ScrollDelegate (IntPtr window, double xOffset, double yOffset);
    /// <summary>
    /// The function signature for window size callback functions.
    /// </summary>
    /// <param name="window">
    /// The window that was resized.
    /// </param>
    /// <param name="width">
    /// The new width, in screen coordinates, of the window.
    /// </param>
    /// <param name="height">
    /// The new height, in screen coordinates, of the window.
    /// </param>
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    public delegate void WindowSizeDelegate (IntPtr window, int width, int height);

}

