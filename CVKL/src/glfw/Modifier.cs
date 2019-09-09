// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
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