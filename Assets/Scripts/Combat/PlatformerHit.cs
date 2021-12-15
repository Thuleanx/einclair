using UnityEngine;

using Thuleanx.Combat;

namespace Thuleanx.Combat.Core {
	public class PlatformerHit : IHit {
		public PlatformerHit(int damage) {
			this.damage = damage;
		}
	}
}