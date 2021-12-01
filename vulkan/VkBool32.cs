// Copyright (c) 2017 Eric Mellino
// Copyright (c) 2019 JP Bruyère
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;

namespace Vulkan
{
    /// <summary>
    /// A boolean value stored in a 4-byte unsigned integer.
    /// </summary>
    public struct VkBool32 : IEquatable<VkBool32>, IConvertible
    {
        /// <summary>
        /// The raw value of the <see cref="VkBool32"/>. A value of 0 represents "false", all other values represent "true".
        /// </summary>
        public uint Value;

        /// <summary>
        /// Constructs a new <see cref="VkBool32"/> with the given raw value. 
        /// </summary>
        /// <param name="value"></param>
        public VkBool32(uint value)
        {
            Value = value;
        }

        /// <summary>
        /// Represents the boolean "true" value. Has a raw value of 1.
        /// </summary>
        public static readonly VkBool32 True = new VkBool32(1);

        /// <summary>
        /// Represents the boolean "true" value. Has a raw value of 0.
        /// </summary>
        public static readonly VkBool32 False = new VkBool32(0);

        /// <summary>
        /// Returns whether another <see cref="VkBool32"/> value is considered equal to this one.
        /// Two <see cref="VkBool32"/>s are considered equal when their raw values are equal.
        /// </summary>
        /// <param name="other">The value to compare to.</param>
        /// <returns>True if the other value's underlying raw value is equal to this instance's. False otherwise.</returns>
        public bool Equals(VkBool32 other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is VkBool32 b && Equals(b);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{(this ? "True" : "False")} ({Value})";
        }

		public TypeCode GetTypeCode() => TypeCode.Boolean;

		public bool ToBoolean(IFormatProvider provider) => (bool)this;
		public byte ToByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public char ToChar(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public DateTime ToDateTime(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public decimal ToDecimal(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}
		public double ToDouble(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}
		public short ToInt16(IFormatProvider provider) => (short)Value;
		public int ToInt32(IFormatProvider provider) => (int)Value;
		public long ToInt64(IFormatProvider provider) => (long)Value;

		public sbyte ToSByte(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public float ToSingle(IFormatProvider provider)
		{
			throw new NotImplementedException();
		}

		public string ToString(IFormatProvider provider) => ToString();

		public object ToType(Type conversionType, IFormatProvider provider)
		{
			if (conversionType == typeof(bool))
				return ToBoolean(null);
			throw new NotImplementedException();
		}

		public ushort ToUInt16(IFormatProvider provider) => (ushort)Value;
		public uint ToUInt32(IFormatProvider provider) => Value;
		public ulong ToUInt64(IFormatProvider provider) => (ulong)Value;

		public static implicit operator bool(VkBool32 b) => b.Value != 0;
        public static implicit operator uint(VkBool32 b) => b.Value;
        public static implicit operator VkBool32(bool b) => b ? True : False;
        public static implicit operator VkBool32(uint value) => new VkBool32(value);

        public static bool operator ==(VkBool32 left, VkBool32 right) => left.Value == right.Value;
        public static bool operator !=(VkBool32 left, VkBool32 right) => left.Value != right.Value;
    }
}
