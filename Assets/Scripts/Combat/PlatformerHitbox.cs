using UnityEngine;

using Thuleanx.Combat;
using Thuleanx.Utils;
using Utils;

namespace Thuleanx.Combat.Core {
	public class PlatformerHitbox : Hitbox {
		public int Damage;
		public float KnockbackForce;
		[SerializeField] Optional<Vector2> knockbackDir;
		public Vector2 KnockbackDir => (knockbackDir.Value * (Vector2) transform.localScale).normalized;
		public bool Active => State == ColliderState.Open;

		[EnumFlag("Thuleanx.Combat.Core.HitLayer")]
		public HitLayer HitMask;

		public override IHit generateHit(Collider2D collision) {
			if (knockbackDir.Enabled) {
				return new PlatformerHit(Damage, KnockbackForce * KnockbackDir * transform.lossyScale);
			} else {
				Vector2 backDir = (collision.gameObject.transform.position - transform.position).normalized;
				return new PlatformerHit(Damage, KnockbackForce * backDir * transform.lossyScale);
			}
		}
		protected override bool CanCollide(Hurtbox hurtbox)
			=> (hurtbox is PlatformerHurtbox) && (HitMask & (hurtbox as PlatformerHurtbox).Layer) > 0;

		private void OnDrawGizmosSelected() {
			Gizmos.color = State == Hitbox.ColliderState.Closed ? Color.green : Color.red;
			Gizmos.DrawCube(transform.position, Vector3.one/4f);
			if (knockbackDir.Enabled) DrawArrow.ForGizmo(transform.position, KnockbackDir * (Vector2) transform.lossyScale);
		}
	}
}