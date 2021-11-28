using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using Thuleanx.Utils;

namespace Thuleanx.Input.Core {
	[DisallowMultipleComponent]
	public class InputSystem : MonoMiddleware {
		public static float InputBufferTime = .2f;

		public static InputSystem Instance;

		Timer Jump, JumpReleased;
		Vector2 Movement;

		void Awake() {
			Instance = this;

			Jump = new Timer(InputBufferTime);
			JumpReleased = new Timer(InputBufferTime);
		}

		public void OnMoveInput(InputAction.CallbackContext context) {
			if (context.performed) Movement = context.ReadValue<Vector2>();
			if (context.canceled) Movement = Vector2.zero;
		}
		public void OnJumpInput(InputAction.CallbackContext context) {
			if (context.started) Jump.Start();
			if (context.performed) JumpReleased.Start();
		}

		public override InputState Process(InputState state) {
			PlayerInputState pis = state as PlayerInputState;

			pis.Movement = Movement;
			pis.Jump = Jump;
			pis.JumpReleased = JumpReleased;

			return state;
		}

		public override void Review(InputFeedback feedback) {
			PlayerInputFeedback pfeedback = feedback as PlayerInputFeedback;
			if (pfeedback.JumpExecuted) Jump.Stop();
			if (pfeedback.JumpReleaseExecuted) JumpReleased.Stop();
		}


		public override int GetPriority() => (int) MiddlewarePriority.INPUT;
	}
}