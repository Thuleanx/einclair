using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using Thuleanx.Utils;

namespace Thuleanx.Input.Core {
	[DisallowMultipleComponent]
	public class HammerdudeInputSystem : PlatformerMonoMiddleware {
		public static float InputBufferTime = .2f;

		public static HammerdudeInputSystem Instance;

		Timer Attack, Bash;
		bool AttackHold = false;
		Vector2 Movement;

		public override void Awake() {
			base.Awake();
			Instance = this;

			Attack = new Timer(InputBufferTime);
			Bash = new Timer(InputBufferTime);
		}

		public void OnMoveInput(InputAction.CallbackContext context) {
			if (context.performed) Movement = context.ReadValue<Vector2>();
			if (context.canceled) Movement = Vector2.zero;
		}
		public void OnAttack(InputAction.CallbackContext ctx) {
			if (ctx.started) {
				AttackHold = true;
			}
			if (ctx.canceled) {
				AttackHold = false;
				Attack.Start();
			}
		}
		public void OnBash(InputAction.CallbackContext ctx) {
			if (ctx.started) Bash.Start();
		}

		public override InputState Process(InputState state) {
			HammerdudeInputState InputState = state as HammerdudeInputState;
			InputState.Movement = Movement;
			InputState.Attack = Attack || AttackHold;
			InputState.Bash = Bash;
			return state;
		}

		public override void Review(InputFeedback feedback) {
			HammerdudeInputFeedback InputFeedback = feedback as HammerdudeInputFeedback;
			if (InputFeedback.AttackExecuted) Attack.Stop();
			if (InputFeedback.BashExecuted) Bash.Stop();
		}


		public override int GetPriority() => (int) MiddlewarePriority.INPUT;
	}
}