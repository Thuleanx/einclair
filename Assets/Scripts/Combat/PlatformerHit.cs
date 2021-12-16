using UnityEngine;

using Thuleanx.Combat;

namespace Thuleanx.Combat.Core {
	public class PlatformerHit : IHit {
		public Vector2 KnockbackForce;

		public PlatformerHit(int damage, Vector2 knockback) {
			this.damage = damage;
			this.KnockbackForce = knockback;
		}
	}
}