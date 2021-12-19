using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

using System.Collections;

using Thuleanx.AI.Core;

namespace Thuleanx.Manager.Core {
	[RequireComponent(typeof(PlayableDirector))]
	public class RespawnManager : MonoBehaviour {
		[HideInInspector] public RespawnPoint Point;
		public UnityEvent OnRespawn;

		public void Respawn() {
			OnRespawn?.Invoke();
			GetComponent<PlayableDirector>().Stop();
			GetComponent<PlayableDirector>().Play();
		}
		public void PlaceCharacter() {
			LivePlatformerAI Player = Context.ReferenceManager.Player;
			if (Point) Player.Body.SetPosition(Point.transform.position);
			Player._FullHeal();
		}
	}
}