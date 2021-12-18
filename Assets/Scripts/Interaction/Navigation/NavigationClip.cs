using UnityEngine;
using UnityEngine.Playables;

using System.Collections.Generic;

using Thuleanx.Input.Core;

namespace Thuleanx.Interaction.Core {
	public class NavigationClip : PlayableAsset {
		public PlatformerInputProvider InputProvider;
		public Vector2 Offset;
		public bool PauseTillDone = true;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
			var playable = ScriptPlayable<NavigationBehaviour>.Create(graph);

			NavigationBehaviour behaviour = playable.GetBehaviour();

			Vector2 scaledOffset = Offset;
			scaledOffset.x *= owner.transform.lossyScale.x;
			scaledOffset.y *= owner.transform.lossyScale.y;

			behaviour.Position = (Vector2) owner.transform.position + scaledOffset;
			behaviour.InputProvider = InputProvider;
			behaviour.PauseTillDone = PauseTillDone;

			return playable;
		}
	}
}
