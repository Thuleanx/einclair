using UnityEngine;
using UnityEngine.Events;

using System.Collections;

using Thuleanx.AI.Core;

namespace Thuleanx.Manager.Core {
	public class RespawnManager : MonoBehaviour {
		[HideInInspector] public RespawnPoint Point;
		public UnityEvent OnRespawn;

		public void Respawn() {
			OnRespawn?.Invoke();
		}
		public void PlaceCharacter() {
			LivePlatformerAI Player = Context.ReferenceManager.Player;
			if (Point) Player.Body.SetPosition(Point.transform.position);
			Player._FullHeal();
		}
	}
}