using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Thuleanx.Optimization;

using System.Collections;

using Thuleanx.AI.Core;

namespace Thuleanx.Manager.Core {
	[RequireComponent(typeof(PlayableDirector))]
	public class RespawnManager : MonoBehaviour {
		[HideInInspector] public RespawnPoint Point;
		public UnityEvent OnRespawn;
		public BubblePool CorpsePool;

		public void Respawn() {
			OnRespawn?.Invoke();
			GetComponent<PlayableDirector>().Stop();
			GetComponent<PlayableDirector>().Play();
		}
		public void PlaceCharacter() {
			LivePlatformerAI Player = Context.ReferenceManager.Player;
			if (CorpsePool) {
				GameObject obj = CorpsePool.Borrow(gameObject.scene, Player.transform.position);
				obj.transform.localScale = Player.transform.localScale;
			}
			if (Point) Player.Body.SetPosition(Point.transform.position);
			Player._FullHeal();
		}
	}
}