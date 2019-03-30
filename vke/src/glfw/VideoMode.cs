using System.Runtime.InteropServices;

namespace Glfw
{
    /// <summary>
    /// Represents a single video mode.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoMode
    {
        private int width;

        private int height;

        private int redBits;

        private int greenBits;

        private int blueBits;

        /// <summary>
        /// The resolution, in screen coordinates, of the video mode.
        /// </summary>
        public (int Width, int Height) Resolution => (this.width, this.height);

        /// <summary>
        /// The bit depth of the red channel of the video mode.
        /// </summary>
        public (int Red, int Green, int Blue) Bits => (this.redBits, this.greenBits, this.blueBits);

        /// <summary>
        /// The refresh rate, in Hz, of the video mode.
        /// </summary>
        public int RefreshRate;

        /// <summary>
        /// Returns a string representation of this video mode.
        /// </summary>
        /// <returns>
        /// A string representation of this video mode.
        /// </returns>
        public override string ToString()
        {
            return $"[Resolution: {this.Resolution}, Bit Depth: {this.Bits}, Refresh Rate: {this.RefreshRate}Hz]";
        }
    }
}
