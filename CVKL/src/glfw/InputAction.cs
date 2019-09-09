// Copyright (c) 2019 Andrew Armstrong/FacticiusVir
// Copyright (c) 2019 Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
namespace Glfw
{
    /// <summary>
    /// Represents the action of an input key event or the state of a key.
    /// </summary>
    public enum InputAction
    {
        /// <summary>
        /// The key was released or is not pressed.
        /// </summary>
        Release = 0,
        /// <summary>
        /// The key was or is pressed.
        /// </summary>
        Press = 1,
        /// <summary>
        /// The key has been held down long enough to repeat.
        /// </summary>
        Repeat = 2
    }
}
