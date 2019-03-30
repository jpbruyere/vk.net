using System;

namespace Glfw
{
    /// <summary>
    /// Bitmask indicating modifer keys.
    /// </summary>
    [Flags]
    public enum Modifier
    {
        Shift = 0x1,
        Control = 0x2,
        Alt = 0x4,
        Super = 0x8,
    }
}