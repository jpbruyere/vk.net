// Copyright (c) 2019  Jean-Philippe Bruyère <jp_bruyere@hotmail.com>
//
// This code is licensed under the MIT license (MIT) (http://opensource.org/licenses/MIT)
using System.Collections.Generic;
using VK;

namespace CVKL {
	public class DescriptorSet {
        internal VkDescriptorSet handle;
		public VkDescriptorSet Handle => handle;
        DescriptorPool pool;
        internal List<VkDescriptorSetLayout> descriptorSetLayouts = new List<VkDescriptorSetLayout> ();

        public DescriptorSet (DescriptorPool descriptorPool) {
            pool = descriptorPool;
        }
        public DescriptorSet (DescriptorPool descriptorPool, params DescriptorSetLayout[] layouts) 
            : this (descriptorPool) {

            foreach (DescriptorSetLayout layout in layouts)
                descriptorSetLayouts.Add (layout.handle);
        }        

		public void Free () {
			pool.FreeDescriptorSet (this);
		}
    }
}
