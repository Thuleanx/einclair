using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlatformerInputFeedback : InputFeedback {
		public bool JumpExecuted;
		public bool JumpReleaseExecuted;

		public virtual void Reset() {
			JumpExecuted = false;
			JumpReleaseExecuted = false;
		}
	}
}