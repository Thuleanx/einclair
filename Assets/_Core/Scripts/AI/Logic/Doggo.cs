using UnityEngine;

using Thuleanx.Input;
using Thuleanx.Input.Core;

using Thuleanx.Utils;

namespace Thuleanx.AI.Core {
	public class Doggo : MonoMiddleware {
		[SerializeField] float AttackCooldown = 4f;
		Timer _attackCD;

		public override int GetPriority() => (int) MiddlewarePriority.AI;
		public override InputState Process(InputState state) {
			if (!(state is AttackerInputState)) return state;

			AttackerInputState InputState = state as AttackerInputState;

			Vector2 targetPosition = Context.ReferenceManager.Player.transform.position;
			float dx = targetPosition.x - transform.position.x;

			InputState.Movement.x = Mathf.Abs(dx) < .1f ? 0 : Mathf.Sign(dx);
			if  (!_attackCD) InputState.Attack = true;

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
	}
}