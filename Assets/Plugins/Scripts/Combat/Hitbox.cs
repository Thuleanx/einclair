using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using Thuleanx.AI;

namespace Thuleanx.Combat {
	[RequireComponent(typeof(Collider2D))]
	public abstract class Hitbox : MonoBehaviour, IHitbox {
		protected enum ColliderState {
			Open,
			Closed
		}

		Collider2D Collider;
		public bool DefaultActive;

		[SerializeField, Min(0f)] float Frequency;

		Dictionary<long,float> HurtboxCD = new Dictionary<long,float>();
		ColliderState _state;
		protected ColliderState State {
			get => _state;
			set {
				_state = value;
			}
		}

		public virtual void Awake() {
			Collider = GetComponent<Collider2D>();
			Reset();
			if (DefaultActive) 	startCheckingCollision();
			else 				stopCheckingCollision();
		}
		public void Reset() => HurtboxCD.Clear();
		void OnEnable() => Reset();
		void OnDisable() => Reset();

		public void startCheckingCollision() => State = ColliderState.Open;
		public void stopCheckingCollision() {
			State = ColliderState.Closed;
			HurtboxCD.Clear();
		} 

		void FixedUpdate() {
			if (State != ColliderState.Closed) {
				List<Collider2D> collisions = new List<Collider2D>();
				ContactFilter2D filter = new ContactFilter2D();
				filter.layerMask = Physics2D.GetLayerCollisionMask(gameObject.layer);
				Physics2D.OverlapCollider(Collider, filter, collisions);
				foreach (var other in collisions) {
					Hurtbox hurtbox = other.gameObject.GetComponent<Hurtbox>();
					if (hurtbox != null && CanCollide(hurtbox) && TimedOut(hurtbox.ID)) {
						hurtbox.ApplyHit(generateHit(other));
						Refresh(hurtbox.ID);
					}
				}
			}
		}

		void OnTriggerStay2D(Collider2D other) {
		}

		protected virtual bool CanCollide(Hurtbox hurtbox) => true;
		bool TimedOut(long hurtboxID) => !HurtboxCD.ContainsKey(hurtboxID) 
			|| (Frequency > 0 && HurtboxCD[hurtboxID] + Frequency < Time.time);
		void Refresh(long hurtboxID) => HurtboxCD[hurtboxID] = Time.time;

		public abstract IHit generateHit(Collider2D collision);

	}
}