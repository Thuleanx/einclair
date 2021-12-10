using UnityEngine;
using UnityEngine.Events;

namespace Thuleanx.Interaction {
	[RequireComponent(typeof(Collider2D))]
	public class CollisionTrigger : MonoBehaviour {
		public UnityEvent Trigger;

		void OnTriggerEnter2D(Collider2D other) {
			Trigger?.Invoke();
		}
	}
}