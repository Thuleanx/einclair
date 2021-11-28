using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlayerInputFeedback : InputFeedback {
		public bool JumpExecuted;
		public bool JumpReleaseExecuted;
		public bool InteractExecuted;

		public void Reset() {
			JumpExecuted = false;
			JumpReleaseExecuted = false;
			InteractExecuted = false;
		}
	}
}