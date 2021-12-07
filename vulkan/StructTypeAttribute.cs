// Copyright (c) 2021  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System;

namespace Vulkan
{
    /// <summary>
    /// tel the rewriter to automatically set the structure type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class StructureTypeAttribute : Attribute
    {
        int structureType;
        public StructureTypeAttribute (int value) : base() {
            structureType = value;
        }
        public int StructureType => structureType;
    }
}
