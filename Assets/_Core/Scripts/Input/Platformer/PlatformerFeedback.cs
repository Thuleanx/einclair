using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlatformerINputFeedback : InputFeedback {
		public bool JumpExecuted;
		public bool JumpReleaseExecuted;

		public void Reset() {
			JumpExecuted = false;
			JumpReleaseExecuted = false;
		}
	}
}