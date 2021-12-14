using UnityEngine;
using UnityEngine.Events;

using Thuleanx;
using Draggable;


namespace Thuleanx.Interaction.Core {
	public class PlayerDetector : MonoBehaviour {
		public float Range = 3f;
		[HideInInspector] public bool Detected = false;
		[DraggablePoint(true)] public Vector3 offset;

		void Update() {
			Detected = (Context.ReferenceManager.Player.transform.position - (transform.position + offset)).magnitude <= Range;
		}

		void OnDrawGizmos() {
			if (Detected) Gizmos.color = Color.white;
			else Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(transform.position + offset, Range);
		}
	}
}