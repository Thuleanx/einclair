using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlayerInputState : InputState {
		public Vector2 Movement;
		public bool Jump;
		public bool JumpReleased;
	}
}