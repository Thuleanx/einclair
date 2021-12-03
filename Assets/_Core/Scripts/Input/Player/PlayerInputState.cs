using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlayerInputState : PlatformerInputState {
		public Vector2 TargetPosition;
		public bool Attack;
		public bool Interact;
		public bool CanInteract;
		public bool Jump;
		public bool JumpReleased;
	}
}