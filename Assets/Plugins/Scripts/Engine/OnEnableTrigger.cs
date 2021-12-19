using UnityEngine;
using UnityEngine.Events;

namespace Thuleanx.Engine {
	public class OnEnableTrigger : MonoBehaviour {
		public UnityEvent Trigger;
		void OnEnable() {
			Trigger?.Invoke();
		}
	}
}