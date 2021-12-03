using UnityEngine;

namespace Thuleanx.Engine {
	[DisallowMultipleComponent]
	public class Teleporte : MonoBehaviour {
		public Teleporte Destination;

		public void Teleport() {
			if (Destination != null && Context.ReferenceManager.Player) {
				Debug.Log("HI");
				Context.ReferenceManager.Player.Body.SetPosition(Destination.transform.position);
			}
		}
	}
}