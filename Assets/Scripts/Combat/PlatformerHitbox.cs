using UnityEngine;

using Thuleanx.Combat;
using Thuleanx.Utils;
using Utils;

namespace Thuleanx.Combat.Core {
	public class PlatformerHitbox : Hitbox {
		public int Damage;
		public float KnockbackForce;
		[SerializeField] Vector2 knockbackDir = Vector2.right;
		public Vector2 KnockbackDir => (knockbackDir * (Vector2) transform.localScale).normalized;

		[EnumFlag("Thuleanx.Combat.Core.HitLayer")]
		public HitLayer HitMask;

		public override IHit generateHit(Collider2D collision) {
			return new PlatformerHit(Damage, KnockbackForce * KnockbackDir);
		}
		protected override bool CanCollide(Hurtbox hurtbox)
			=> (hurtbox is PlatformerHurtbox) && (HitMask & (hurtbox as PlatformerHurtbox).Layer) > 0;

		private void OnDrawGizmosSelected() {
			Gizmos.color = State == Hitbox.ColliderState.Closed ? Color.green : Color.red;
			Gizmos.DrawCube(transform.position, Vector3.one/4f);
			DrawArrow.ForGizmo(transform.position, KnockbackDir * (Vector2) transform.lossyScale);
		}
	}
}