using UnityEngine;
using UnityEngine.Events;

namespace Thuleanx.Interaction {
	[RequireComponent(typeof(Collider2D))]
	public class CollisionTrigger : MonoBehaviour {
		public bool Once = false;
		public UnityEvent Trigger;
		bool triggered = false;

		void OnEnable() {
			triggered = false;
		}

		void OnTriggerEnter2D(Collider2D other) {
			if (other.gameObject.tag == "Player") {
				if (!Once || !triggered) Trigger?.Invoke();
				triggered = true;
			}
		}
	}
}