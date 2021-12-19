using UnityEngine;

namespace Thuleanx.Manager.Core {
	public class RespawnPoint : MonoBehaviour {
		public void Register() {
			Context.RespawnManager.Point = this;
		}
	}
}