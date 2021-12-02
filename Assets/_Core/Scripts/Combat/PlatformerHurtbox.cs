using UnityEngine;

using Thuleanx.Combat;
using Thuleanx.AI.Core;

namespace Thuleanx.Combat.Core {
	public class PlatformerHurtbox : Hurtbox {
		public HitLayer Layer;

		public override void ApplyHit(IHit hit) => GetComponentInParent<LivePlatformerAI>()?.ApplyHit(hit);
		public override bool CanTakeHit() {
			LivePlatformerAI AI = GetComponentInParent<LivePlatformerAI>();
			if (AI) return AI.CanTakeHit();
			return true;
		}
	}
}