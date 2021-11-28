using UnityEngine;
using UnityEngine.Events;

using Thuleanx;

namespace Thuleanx.Interaction.Core {
	public class PlayerDetector : MonoBehaviour {
		public float Range = 3f;
		[HideInInspector]
		public bool Detected = false;

		void Update() {
			Detected = (Context.ReferenceManager.Player.transform.position - transform.position).magnitude <= Range;
		}

		void OnDrawGizmos() {
			if (Detected) Gizmos.color = Color.white;
			else Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(transform.position, Range);
		}
	}
}