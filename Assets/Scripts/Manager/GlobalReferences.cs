using UnityEngine;

using Thuleanx.Input.Core;

namespace Thuleanx.Manager.Core {
	public class GlobalReferences : MonoBehaviour {
		public static GlobalReferences Instance;

		[SerializeField] PlayerInputProvider _PlayerInputProvider;

		public static PlayerInputProvider PlayerInputProvider => Instance._PlayerInputProvider;

		void Awake() {
			Instance = this;
		}
	}
}