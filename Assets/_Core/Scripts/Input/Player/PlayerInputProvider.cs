using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	[CreateAssetMenu(fileName = "PIP", menuName = "~Einclair/PlayerInputProvider", order = 0)]
	public class PlayerInputProvider : PlatformerInputProvider {
		public new PlayerInputFeedback Feedback = new PlayerInputFeedback();

		public override InputState GetState() {
			PlayerInputState InputState = new PlayerInputState();
			foreach (var kvp in Middlewares)
				InputState = kvp.Value.Process(InputState) as PlayerInputState;
			return InputState;
		}

		public override void ProcessFeedback() {
			foreach (var kvp in Middlewares)
				kvp.Value.Review(Feedback);
			Feedback.Reset();
		}
	}
}