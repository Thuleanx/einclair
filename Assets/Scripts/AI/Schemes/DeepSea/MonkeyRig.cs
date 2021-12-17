using UnityEngine;
using System;

using Thuleanx.Input.Core;
using Thuleanx.Combat.Core;

using MarkupAttributes;

namespace Thuleanx.AI.Core {
	[RequireComponent(typeof(Animator))]
	public class MonkeyRig : LivePlatformerAI {
		public enum State {
			Normal = 0,
			Dead = 1,
			Attack = 2,
			Fly = 3
		}

		AttackerInputState InputState;

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(State)).Length, (int) State.Normal);

			// ============================= Normal ============================
			StateMachine.SetCallbackUpdate((int) State.Normal, NormalUpdate);
			StateMachine.SetCallbackEnd((int) State.Normal, NormalExit);

			// ============================= Attack ============================
			StateMachine.SetCallbackBegin((int) State.Attack, _State_AttackEnter);
			StateMachine.SetCallbackTransition((int) State.Attack, _State_AttackTransition);
			StateMachine.SetCallbackUpdate((int) State.Attack, _State_AttackUpdate);
			StateMachine.SetCallbackEnd((int) State.Attack, _State_AttackExit);
		}

		public override void ObjectSetup() {
			base.ObjectSetup();
			_originalParent = transform.parent;
			if (Provider == null) Provider = ScriptableObject.CreateInstance<AttackerInputProvider>();
		}

		public override void Update() {
			InputState = Provider.GetState() as AttackerInputState;
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
		public virtual int NormalUpdate() {
			Vector2 Movement = InputState.Movement;

			// Platform code
			_isOnPlatform = PlatformCheck();
			transform.parent = _platform ? _platform : _originalParent;

			// Horizontal
			{
				float current = Body.Velocity.x;
				float intention = Movement.x * baseMovementSpeed;
				if (Mathf.Abs(Movement.x) <= 0.01f) intention = 0f;

				Body.AccelerateTowards(new Vector2(intention, Body.Velocity.y));

				if (InputState.Attack)
					return (int) State.Attack;

				CorrectTurn();

			}

			return -1;
		}
		protected void NormalExit() {
			_isOnPlatform = false;
			transform.parent = _platform ? _platform : _originalParent;
		}
		#endregion

		#region Attack
		[Box("Attack")]
		[Min(0f)] public float Attack_ForwardPush;
		public PlatformerHitbox AttackHitbox;
		[EndGroup("Attack")]

		bool _attackEnded;
		bool _attackExecuted;

		void _State_AttackEnter() { 
			_attackEnded = false;
			_attackExecuted = false;
		}
		int _State_AttackTransition() => _attackEnded ? (int) State.Normal : -1;
		int _State_AttackUpdate() {
			if (!_attackExecuted) CorrectTurn();
			Body.AccelerateTowards(new Vector2(0, Body.Velocity.y));
			return -1;
		}
		void _State_AttackExit() {
			AttackHitbox?.stopCheckingCollision();
		}
		public void _Animation_OnAttack() {
			Body.Knockback(new Vector2(Attack_ForwardPush * (IsRightFacing ? 1 : -1), 0));
			_attackExecuted = true;
			(Provider.Feedback as AttackerInputFeedback).AttackExecuted = true;
		}
		public void _Animation_OnAttackEnd() {
			_attackEnded = true;
		}
		#endregion

		public void CorrectTurn() {
			if (InputState.Movement.x < 0 && (_isFacingRight ^ defaultLeftFacing)) Flip();
			else if (InputState.Movement.x > 0 && (!_isFacingRight ^ defaultLeftFacing)) Flip();
		}
	}
}