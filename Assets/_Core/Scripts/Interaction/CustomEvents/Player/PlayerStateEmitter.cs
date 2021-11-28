using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

using Thuleanx.AI.Core;

namespace Thuleanx.Interaction.Core {
	public class PlayerStateEmitter : SignalEmitter {
		public Player.PlayerState State;

		public void Execute() {
			// Force state!! Be careful as we don't know when this is activated
			if (Context.ReferenceManager.Player)
				Context.ReferenceManager.Player.StateMachine.State = (int) State;
		}
	}
}