namespace Vulkan 
{
    public struct Version
    {
        readonly uint value;

        public Version(uint major, uint minor, uint patch) {
            value = major << 22 | minor << 12 | patch;
        }
		public Version (uint value) {
			this.value = value;
		}

		public uint Major => value >> 22;
        public uint Minor => (value >> 12) & 0x3ff;
        public uint Patch => (value >> 22) & 0xfff;

        public static implicit operator uint(Version version) => version.value;
		public static implicit operator Version (uint version) => new Version(version);
	}
}