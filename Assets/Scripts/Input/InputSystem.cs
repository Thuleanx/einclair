using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using Thuleanx.Utils;

namespace Thuleanx.Input.Core {
	[DisallowMultipleComponent]
	public class InputSystem : PlatformerMonoMiddleware {
		public static float InputBufferTime = .2f;

		public static InputSystem Instance;

		Vector2 MouseScreenPos;
		public Vector2 MouseWorldPos {
			get {
				if (Camera.main != null)
					return Camera.main.ScreenToWorldPoint(MouseScreenPos);
				return Vector2.zero;
			}
		}

		Timer Jump, JumpReleased, Interact, Attack;
		Vector2 Movement;

		public override void Awake() {
			base.Awake();
			Instance = this;

			Jump = new Timer(InputBufferTime);
			JumpReleased = new Timer(InputBufferTime);
			Interact = new Timer(InputBufferTime);
			Attack = new Timer(InputBufferTime);
		}

		public void OnMoveInput(InputAction.CallbackContext context) {
			if (context.performed) Movement = context.ReadValue<Vector2>();
			if (context.canceled) Movement = Vector2.zero;
		}
		public void OnJumpInput(InputAction.CallbackContext context) {
			if (context.started) Jump.Start();
			if (context.performed) JumpReleased.Start();
		}
		public void OnInteract(InputAction.CallbackContext context) {
			if (context.started) Interact.Start();
		}
		public void OnMouseInput(InputAction.CallbackContext ctx) => 
			MouseScreenPos = ctx.ReadValue<Vector2>();
		public void OnAttack(InputAction.CallbackContext ctx) {
			if (ctx.started) Attack.Start();
		}

		public override InputState Process(InputState state) {
			PlayerInputState pis = state as PlayerInputState;

			// Debug.Log(MouseScreenPos + " " + Camera.main.transform.position + " " + MouseWorldPos);

			pis.TargetPosition = MouseWorldPos;
			pis.Movement = Movement;
			pis.Jump = Jump;
			pis.JumpReleased = JumpReleased;
			pis.Interact = Interact;
			pis.Attack = Attack;

			pis.CanInteract = true; // might be overwritten later, kek

			return state;
		}

		public override void Review(InputFeedback feedback) {
			PlayerInputFeedback pfeedback = feedback as PlayerInputFeedback;
			if (pfeedback.JumpExecuted) Jump.Stop();
			if (pfeedback.JumpReleaseExecuted) JumpReleased.Stop();
			if (pfeedback.InteractExecuted) Interact.Stop();
			if (pfeedback.AttackExecuted) Attack.Stop();
		}


		public override int GetPriority() => (int) MiddlewarePriority.INPUT;
	}
}