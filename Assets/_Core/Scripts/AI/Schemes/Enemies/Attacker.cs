using UnityEngine;
using UnityEngine.Events;

using Thuleanx.Utils;
using Thuleanx.Input.Core;
using Thuleanx.Combat;

using MarkupAttributes;

namespace Thuleanx.AI.Core {
	public class Attacker : BasicAI {
        public enum AttackerState {
            Normal = 0,
            Attack = 1
        }

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(2, 0);
			StateMachine.SetCallbackUpdate((int) AttackerState.Normal, NormalUpdate);
			StateMachine.SetCallbackEnd((int) AttackerState.Normal, NormalExit);

			StateMachine.SetCallbackTransition((int) AttackerState.Attack, AttackTransition);
			StateMachine.SetCallbackBegin((int) AttackerState.Attack,AttackEnter);
			StateMachine.SetCallbackEnd((int) AttackerState.Attack, AttackExit);
			StateMachine.SetCallbackUpdate((int) AttackerState.Attack, AttackUpdate);
		}

		public override void ObjectSetup() {
			if (Provider == null) Provider = ScriptableObject.CreateInstance<AttackerInputProvider>();
			base.ObjectSetup();
		}

		public override void Update() {
			base.Update();
			AnimationUpdate();
		}

		void AnimationUpdate() {
			switch (StateMachine.State) {
				case (int) AttackerState.Normal:
					Anim.SetInteger("State", 0);
					break;
				case (int) AttackerState.Attack:
					Anim.SetInteger("State", 1);
					break;
			}
			Anim.SetFloat("VelocityX", Body.Velocity.x);
			Anim.SetFloat("VelocityY", Body.Velocity.y);
		}

		public override int NormalUpdate() {
			if (InputState is AttackerInputState && (InputState as AttackerInputState).Attack)
				return (int) AttackerState.Attack;
			return base.NormalUpdate();
		}

		#region Attack

		float _lungeVelX;
		[Box("Attack")]
		public Vector2 Lunge;
		public UnityEvent AttackStart;
		public UnityEvent AttackTrigger;
		public UnityEvent AttackEnd;

		bool _AttackAnimationFinish = false;

		public void AttackFinishTrigger() => _AttackAnimationFinish = true;
		protected int AttackTransition() => _AttackAnimationFinish ? (int) AttackerState.Normal : -1;
		protected void AttackEnter() {
			(Provider.Feedback as AttackerInputFeedback).AttackExecuted = true;
			_AttackAnimationFinish = false;
			AttackStart?.Invoke();
			Vector2 LungeTowards = Lunge;
			if (!IsRightFacing) LungeTowards.x *= -1;
			Body.Velocity = LungeTowards;
			_lungeVelX = LungeTowards.x;
		}
		protected int AttackUpdate() {
			Body.SetVelocityX(_lungeVelX);
			return -1;
		}
		protected void AttackExit() {
			AttackEnd?.Invoke();
		}
		public void TriggerableAttack() {
			AttackTrigger?.Invoke();
		}
		#endregion

		void LateUpdate() {
			Provider.ProcessFeedback();
		}
	}
}