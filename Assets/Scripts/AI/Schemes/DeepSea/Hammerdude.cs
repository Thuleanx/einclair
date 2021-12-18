using UnityEngine;
using System;

using Thuleanx.Utils;
using Thuleanx.Input;
using Thuleanx.Input.Core;
using Thuleanx.Manager.Core;

using Thuleanx.Combat.Core;

using MarkupAttributes;

namespace Thuleanx.AI.Core {
	[RequireComponent(typeof(Animator))]
	public class Hammerdude : LivePlatformerAI, InputMiddleware {
		public enum State {
			Normal = 0,
			Lock = 1,
			Dead = 2,
			Slam = 3,
			Bash = 4
		}

		HammerdudeInputState InputState;

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(State)).Length, (int) State.Normal);

			// ====================== NORMAL ===============================
			StateMachine.SetCallbackTransition((int) State.Normal, _State_NormalTransition);
			StateMachine.SetCallbackUpdate((int) State.Normal, _State_NormalUpdate);
			StateMachine.SetCallbackEnd((int) State.Normal, _State_NormalExit);

			// ====================== Slam ===============================
			StateMachine.SetCallbackBegin((int) State.Slam, _State_SlamEnter);
			StateMachine.SetCallbackTransition((int) State.Slam, _State_SlamTransition);
			StateMachine.SetCallbackUpdate((int) State.Slam, _State_SlamUpdate);
			StateMachine.SetCallbackEnd((int) State.Slam, _State_SlamExit);

			// ====================== Bash ===============================
			StateMachine.SetCallbackBegin((int) State.Bash, _State_BashEnter);
			StateMachine.SetCallbackTransition((int) State.Bash, _State_BashTransition);
			StateMachine.SetCallbackUpdate((int) State.Bash, _State_BashUpdate);
			StateMachine.SetCallbackEnd((int) State.Bash, _State_BashExit);

			// ====================== Death ===============================
			StateMachine.SetCallbackTransition((int) State.Dead, _State_DeadTransition);
			StateMachine.SetCallbackBegin((int) State.Dead, _State_DeadEnter);
		}

		public override void ObjectSetup() {
			base.ObjectSetup();
			_originalParent = transform.parent;
			OnDeath.AddListener(() => {
				StateMachine.State = (int) State.Dead;
			});
		}

		public override void Update() {
			InputState = base.Provider.GetState() as HammerdudeInputState;
			base.Update();
		}

		void LateUpdate() {
			Anim?.SetInteger("State", StateMachine.State);
			Anim?.SetFloat("HorizontalVelocity", Body.Velocity.x);
			Anim?.SetFloat("VerticalVelocity", Body.Velocity.y);
			base.Provider.ProcessFeedback();
		}

		#region Normal
		Transform _originalParent;
		int _State_NormalTransition() {
			return -1;
		}
		int _State_NormalUpdate() {
			Vector2 Movement = InputState.Movement;

			// Platform code
			_isOnPlatform = PlatformCheck();
			transform.parent = _platform ? _platform : _originalParent;

			// Horizontal
			{
				float current = Body.Velocity.x;
				float intention = Movement.x * baseMovementSpeed;
				Body.AccelerateTowards(new Vector2(intention, Body.Velocity.y));

				CorrectTurn();
			}

			if (InputState.Bash) {
				(Provider.Feedback as HammerdudeInputFeedback).AttackExecuted = true;
				return (int) State.Bash;
			}

			if (InputState.Attack) {
				(Provider.Feedback as HammerdudeInputFeedback).AttackExecuted = true;
				return (int) State.Slam;
			}

			return -1;
		}
		void _State_NormalExit() {
			_isOnPlatform = false;
			transform.parent = _platform ? _platform : _originalParent;
		}
		#endregion

		#region Slam
		[Box("Slam")]
		[Min(0f)] public float Slam_ForwardPush;
		public PlatformerHitbox SlamHitbox;
		[EndGroup("Slam")]

		bool _attackEnded;
		void _State_SlamEnter() {
			_attackEnded = false;
		}
		int _State_SlamTransition() => _attackEnded ? (int) State.Normal : -1;
		int _State_SlamUpdate() {
			Body.AccelerateTowards(new Vector2(0, Body.Velocity.y));
			if (InputState.Bash) {
				// Turn into the right direction
				CorrectTurn();
				(Provider.Feedback as HammerdudeInputFeedback).AttackExecuted = true;
				return (int) State.Bash;
			}
			return -1;
		}
		void _State_SlamExit() {
			SlamHitbox?.stopCheckingCollision();
		}
		public void _Animation_OnAttack() {
			Body.Knockback(new Vector2(Slam_ForwardPush * (IsRightFacing ? 1 : -1), 0));
		}
		public void _Animation_OnAttackEnd() {
			_attackEnded = true;
		}
		#endregion

		#region Bash
		[Box("Bash")]
		public float Bash_ForwardPush;
		public PlatformerHitbox BashHitbox;
		[EndGroup("Bash")]

		bool _bashEnd;

		int _State_BashTransition() {
			return _bashEnd ? (int) State.Normal : -1;
		}
		void _State_BashEnter() {
			_bashEnd = false;
			Body.Knockback(new Vector2(Bash_ForwardPush * (IsRightFacing ? 1 : -1), 0));
		}
		int _State_BashUpdate() {
			Body.AccelerateTowards(new Vector2(0, Body.Velocity.y));
			return -1;
		}
		void _State_BashExit() {
			BashHitbox?.stopCheckingCollision();
		}
		public void _Animation_OnBashEnd() {
			_bashEnd = true;
		}
		#endregion

		#region Death
		int _State_DeadTransition() {
			return Health > 0 ? (int) State.Normal : -1;
		}
		void _State_DeadEnter() {
			Body.Stop();
		}
		#endregion

		public int GetPriority() => (int) MiddlewarePriority.PLAYER;
		public InputState Process(InputState state) {
			return state;
		}
		public void Review(InputFeedback feedback) {}
		public void Detach(InputProvider provider) => provider.RemoveMiddleware(this, GetPriority());
		public void Attach(InputProvider provider) => provider.AddMiddleware(this, GetPriority());

		public void CorrectTurn() {
			if (InputState.Movement.x < 0 && (_isFacingRight ^ defaultLeftFacing)) Flip();
			else if (InputState.Movement.x > 0 && (!_isFacingRight ^ defaultLeftFacing)) Flip();
		}
	}
}