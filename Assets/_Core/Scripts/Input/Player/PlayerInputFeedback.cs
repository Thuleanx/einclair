using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlayerInputFeedback : PlatformerInputFeedback {
		public bool InteractExecuted;

		public override void Reset() {
			base.Reset();
			InteractExecuted = false;
		}
	}
}