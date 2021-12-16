using UnityEngine;

using Thuleanx.Input.Core;

namespace Thuleanx.Manager.Core {
	public class GlobalReferences : MonoBehaviour {
		public static GlobalReferences Instance;

		[SerializeField] 
		PlatformerInputProvider _PlayerInputProvider;

		public static PlatformerInputProvider PlayerInputProvider => Instance._PlayerInputProvider;

		void Awake() {
			Instance = this;
		}
	}
}