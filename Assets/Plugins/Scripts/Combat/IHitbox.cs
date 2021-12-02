using UnityEngine;

namespace Thuleanx.Combat {
	public interface IHitbox {
		void startCheckingCollision();
		void stopCheckingCollision();
		void Reset();
		IHit generateHit(Collider2D collision);
	}
}