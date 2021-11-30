using UnityEngine;

using Thuleanx.Input;
using Thuleanx.Input.Core;

namespace Thuleanx.AI.Core {
	public class Korone : MonoMiddleware {
		public override int GetPriority() => (int) MiddlewarePriority.AI;
		public override InputState Process(InputState state) {
			if (state is PlatformerInputState) {
				PlatformerInputState PIS = state as PlatformerInputState;
				PIS.Movement.x = Mathf.Sign((Context.ReferenceManager.Player.transform.position - transform.position).x);
				return PIS;
			}
			return state;
		}
		public override void Review(InputFeedback feedback) {}

		void Start() => Attach(GetComponent<PlatformerAI>().Provider);
	}
}