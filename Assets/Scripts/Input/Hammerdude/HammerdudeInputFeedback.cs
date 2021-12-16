using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class HammerdudeInputFeedback : PlatformerInputFeedback {
		public bool AttackExecuted = false;
		public bool BashExecuted = false;

		public override void Reset() {
			base.Reset();
			AttackExecuted = true;
			BashExecuted = true;
		}
	}
}