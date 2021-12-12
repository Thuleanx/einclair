using UnityEngine;

using Thuleanx.Input;
using Thuleanx.Input.Core;

using Thuleanx.Utils;

using NaughtyAttributes;
using MarkupAttributes;

namespace Thuleanx.AI.Core {
	public class Clone : MonoMiddleware {
		Timer _attackCD;

		[Box("Combat")]
		[SerializeField] float AttackCooldown = 4f;
		public Transform EyePosition;
		public LayerMask PlayerLayer;
		public float VisionRange;
		public float AttackDetectionRange;
		public float ComfortDistance = 2f;

		[Box("Wander")]
		[MinMaxSlider(0, 10f)]
		public Vector2 WanderDuration;
		Timer _wanderCooldown;
		float _wanderDir;

		public override int GetPriority() => (int) MiddlewarePriority.AI;
		public override InputState Process(InputState state) {
			if (!(state is AttackerInputState)) return state;
			
			AttackerInputState InputState = state as AttackerInputState;

			if (PlayerVisible()) {

				Vector2 targetPosition = Context.ReferenceManager.Player.transform.position;
				float dx = targetPosition.x - transform.position.x;
				Debug.Log(ComfortDistance);
				if (Mathf.Abs(dx) >= ComfortDistance)
					InputState.Movement.x = Mathf.Sign(dx);
				else if (dx > 0 != GetComponent<PlatformerAI>().IsRightFacing) InputState.Movement.x = Mathf.Sign(dx);
				else InputState.Movement.x = 0f;
				if  (!_attackCD && PlayerAttackable()) InputState.Attack = true;

			} else {
				// Wander
				if (!_wanderCooldown) {
					float prevDir = _wanderDir;
					while ((_wanderDir = Random.Range(-1, 2)) == prevDir);
					_wanderCooldown = new Timer(Random.Range(WanderDuration.x, WanderDuration.y));
					_wanderCooldown.Start();
				}

				InputState.Movement.x = _wanderDir;
			}

			return InputState;
		}
		public override void Review(InputFeedback feedback) {
			if (feedback is AttackerInputFeedback) {
				AttackerInputFeedback AIF = feedback as AttackerInputFeedback;
				if (AIF.AttackExecuted) {
					_attackCD = new Timer(AttackCooldown);
					_attackCD.Start();
				}
			}
		}

		void Start() => Attach(GetComponent<PlatformerAI>().Provider);

		bool _playerDetected, _playerAttackable;
		void Update() {
			TryDetectPlayer();
			TryDetectPlayerAttackable();
		}
		void TryDetectPlayer() {
			_playerDetected = false;
			Vector2 dir = 
				Context.ReferenceManager.Player.transform.position.x > transform.position.x ? 
				Vector2.right : Vector2.left;
			foreach (var hit in Physics2D.RaycastAll(EyePosition.position, dir, VisionRange, PlayerLayer))
				if (hit.collider.gameObject == Context.ReferenceManager.Player.gameObject) {
					_playerDetected = true;
					return;
				}
		}
		void TryDetectPlayerAttackable() {
			_playerAttackable = false;
			Vector2 dir = 
				Context.ReferenceManager.Player.transform.position.x > transform.position.x ? 
				Vector2.right : Vector2.left;
			foreach (var hit in Physics2D.RaycastAll(EyePosition.position, dir, AttackDetectionRange, PlayerLayer))
				if (hit.collider.gameObject == Context.ReferenceManager.Player.gameObject) {
					_playerAttackable = true;
					return;
				}
		}

		public bool PlayerAttackable() => _playerAttackable;
		public bool PlayerVisible() => _playerDetected;
	}
}