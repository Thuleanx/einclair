using UnityEngine;

namespace Thuleanx.Engine.Core {
	[DisallowMultipleComponent]
	public class Teleporte : MonoBehaviour {
		public Teleporte Destination;

		public void Teleport() {
			if (Destination != null && Context.ReferenceManager.Player)
				Context.ReferenceManager.Player.Body.SetPosition(Destination.transform.position);
		}
	}
}