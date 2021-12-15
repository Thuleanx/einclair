using UnityEngine;

using Thuleanx.Input;
using Thuleanx.Input.Core;

namespace Thuleanx.AI.Core {
	public class Korone : MonoMiddleware {
		public override int GetPriority() => (int) MiddlewarePriority.AI;
		public override InputState Process(InputState state) {
			if (!(state is PlatformerInputState)) return state;

			PlatformerInputState PIS = state as PlatformerInputState;
			float dx = (Context.ReferenceManager.Player.transform.position - transform.position).x;
			PIS.Movement.x = Mathf.Abs(dx) < .1f ? 0 : Mathf.Sign(dx);
			return PIS;
		}
		public override void Review(InputFeedback feedback) {}

		void Start() => Attach(GetComponent<PlatformerAI>().Provider);
	}
}