using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class AttackerInputState : PlatformerInputState {
        public bool Attack;

        public override InputFeedback GetFeedbackBlueprint() => new AttackerInputFeedback();
    }
}