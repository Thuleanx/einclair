using UnityEngine;

using Thuleanx.Manager.Core;

namespace Thuleanx {
	public class Context : MonoBehaviour {
		public static Context Instance;
		
		[SerializeField] ReferenceManager _ReferenceManager;

		public static ReferenceManager ReferenceManager => Instance._ReferenceManager;

		void Awake() {
			Instance = this;
		}
	}
}