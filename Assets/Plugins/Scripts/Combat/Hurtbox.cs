using UnityEngine;

namespace Thuleanx.Combat {
	[RequireComponent(typeof(Collider2D))]
	public abstract class Hurtbox : MonoBehaviour, IHurtable {
		public static long NextID = 0;

		[HideInInspector] public long ID;
		public Collider2D Box { get; private set; }

		void Awake() {
			Box = GetComponent<BoxCollider2D>();
			NextID = ID++;
		}
		public abstract void ApplyHit(IHit hit);
		public abstract bool CanTakeHit();
	}
}