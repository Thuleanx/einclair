using UnityEngine;
using System;

using Thuleanx.Input;
using Thuleanx.Input.Core;

using Thuleanx.Utils;

using MarkupAttributes;

namespace Thuleanx.AI.Core {
	public class Monkey : MonoMiddleware {
		enum State {
			Charge = 0,
			Runaway = 1,
			Idle = 2
		}

		StateMachine StateMachine;
		void Awake() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(State)).Length, (int) State.Idle);

			// ====================== Charge ===============================
			StateMachine.SetCallbackTransition((int) State.Charge, _State_ChargeTransition);

			// ====================== Runaway ===============================
			StateMachine.SetCallbackTransition((int) State.Runaway, _State_RunawayTransition);

			// ====================== Idle ===============================
			StateMachine.SetCallbackTransition((int) State.Idle, _State_IdleTransition);
		}

		[Box("Combat States")]
		[SerializeField] float AttackCooldown = 4f;
		[SerializeField] float AttackDetectionRange = 3f;
		[SerializeField] float RunAwayDuration = 5f;
		[EndGroup("Combat States")]

		[Box("Vision")]
		public float VisionRange = 16f;
		[EndGroup("Vision")]

		Timer _attackCD;
		Timer _feared;

		public override int GetPriority() => (int) MiddlewarePriority.AI;
		public override InputState Process(InputState state) {
			if (!(state is AttackerInputState)) return state;
			AttackerInputState InputState = state as AttackerInputState;

			StateMachine.Update();

			if ((State) StateMachine.State == State.Charge) _State_ChargeProcess(InputState);
			else if ((State) StateMachine.State == State.Runaway) _State_RunawayProcess(InputState);

			return InputState;
		}
		public override void Review(InputFeedback feedback) {
			if (feedback is AttackerInputFeedback) {
				AttackerInputFeedback AIF = feedback as AttackerInputFeedback;
				if (AIF.AttackExecuted) {
					_feared = new Timer(RunAwayDuration);
					_feared.Start();
					_attackCD = new Timer(AttackCooldown);
					_attackCD.Start();
				}
			}
		}

		#region Charge
		void _State_ChargeProcess(AttackerInputState InputState) {
			Vector2 targetPosition = (Vector2) Context.ReferenceManager.Player.transform.position;
			float dir = targetPosition.x - transform.position.x;
			InputState.Movement = (new Vector2(Math.Abs(dir) < AttackDetectionRange ? 0 : Mathf.Sign(dir), 0)) 
				+ new Vector2(Mathf.Sign(dir) * .01f, 0);
			if (PlayerAttackable()) InputState.Attack = !_attackCD;
		}
		int _State_ChargeTransition() {
			if (!PlayerVisible()) return (int) State.Idle;
			if (_feared) return (int) State.Runaway;
			return -1;
		}
		#endregion
		#region Runaway
		void _State_RunawayProcess(AttackerInputState InputState) {
			Vector2 adversePosition = (Vector2) Context.ReferenceManager.Player.transform.position;
			InputState.Movement = new Vector2(Mathf.Sign(transform.position.x - adversePosition.x), 0);
		}
		int _State_RunawayTransition() {
			if (!PlayerVisible()) return (int) State.Idle;
			if (!_feared) return (int) State.Charge;
			return -1;
		}
		#endregion
		int _State_IdleTransition() {
			return PlayerVisible() ? (int) State.Charge : -1;
		}

		void Start() => Attach(GetComponent<PlatformerAI>().Provider);

		bool _playerDetected, _playerAttackable;
		void Update() {
			TryDetectPlayer();
			TryDetectPlayerAttackable();
		}
		void TryDetectPlayer() {
			_playerDetected = Mathf.Abs(Context.ReferenceManager.Player.transform.position.x - transform.position.x) 
				< VisionRange;
		}
		void TryDetectPlayerAttackable() {
			_playerAttackable = Mathf.Abs(Context.ReferenceManager.Player.transform.position.x - transform.position.x) 
				< AttackDetectionRange;
		}

		public bool PlayerAttackable() => _playerAttackable;
		public bool PlayerVisible() => _playerDetected;
	}
}