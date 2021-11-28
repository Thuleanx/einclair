using UnityEngine;
using UnityEngine.Playables;

using System.Collections.Generic;

using Thuleanx.Input.Core;

namespace Thuleanx.Interaction.Core {
	public class NavigationClip : PlayableAsset {
		public PlayerInputProvider InputProvider;
		public Vector2 Offset;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
			var playable = ScriptPlayable<NavigationBehaviour>.Create(graph);

			NavigationBehaviour behaviour = playable.GetBehaviour();
			behaviour.Position = (Vector2) owner.transform.position + Offset;
			behaviour.InputProvider = InputProvider;

			return playable;
		}
	}
}
