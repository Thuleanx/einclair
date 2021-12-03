using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlayerInputFeedback : PlatformerInputFeedback {
		public bool InteractExecuted;
		public bool AttackExecuted;
		public bool JumpExecuted;
		public bool JumpReleaseExecuted;

		public override void Reset() {
			base.Reset();
			JumpExecuted = false;
			JumpReleaseExecuted = false;
			InteractExecuted = false;
			AttackExecuted = false;
		}
	}
}