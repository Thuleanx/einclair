using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlatformerInputState : InputState {
		public Vector2 Movement;

        public virtual InputFeedback GetFeedbackBlueprint() {
			return new PlatformerInputFeedback();
        }
    }
}