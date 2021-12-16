using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class HammerdudeInputState : PlatformerInputState {
		public bool Attack;
		public bool Bash;

        public override InputFeedback GetFeedbackBlueprint() => new HammerdudeInputFeedback();
	}
}