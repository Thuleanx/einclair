using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class AttackerInputFeedback : PlatformerInputFeedback {
        public bool AttackExecuted = false;

		public override void Reset() {
            AttackExecuted = false;
		}
	}
}