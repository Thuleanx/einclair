using UnityEngine;
using UnityEngine.Events;
using  MarkupAttributes;

using Thuleanx.Combat;
using Thuleanx.Combat.Core;

namespace Thuleanx.AI.Core {
	public class LivePlatformerAI : PlatformerAI, IHurtable {
		[Box("Status")]
		public UnityEvent OnHit;
		public int MaxHealth;
		public int Health {
			get => _health;
			private set => Mathf.Clamp(value, 0, MaxHealth);
		}
		public bool IsDead => Health == 0;
		int _health;

		public override void ObjectSetup() {
			base.ObjectSetup();
			Health = MaxHealth;
		}

		public bool CanTakeHit() => !IsDead;
		public void ApplyHit(IHit hit) {
			if (hit is PlatformerHit)
				Body.Knockback((hit as PlatformerHit).KnockbackForce);
			Health -= hit.damage;
			OnHit?.Invoke();
		}
	}
}