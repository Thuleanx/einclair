using UnityEngine;

namespace Utils {
	public class EnumFlagAttribute : PropertyAttribute
	{
		public string enumName;

		public EnumFlagAttribute() {}

		public EnumFlagAttribute(string name)
		{
			enumName = name;
		}
	}
}