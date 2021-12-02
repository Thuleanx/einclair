using UnityEngine;

using Thuleanx.Combat;
using Utils;

namespace Thuleanx.Combat.Core {
	public class PlatformerHitbox : Hitbox {
		public int Damage;

		[EnumFlag("Thuleanx.Combat.Core.HitLayer")]
		public HitLayer HitMask;

		public override IHit generateHit(Collider2D collision) {
			return new PlatformerHit(Damage);
		}
		protected override bool CanCollide(Hurtbox hurtbox)
			=> (hurtbox is PlatformerHurtbox) && (HitMask & (hurtbox as PlatformerHurtbox).Layer) > 0;
	}
}