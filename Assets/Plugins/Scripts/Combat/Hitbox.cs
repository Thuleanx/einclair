using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using Thuleanx.AI;

namespace Thuleanx.Combat {
	[RequireComponent(typeof(Collider2D))]
	public abstract class Hitbox : MonoBehaviour, IHitbox {
		enum ColliderState {
			Open,
			Closed
		}

		Collider2D Collider;
		public bool DefaultActive;

		[SerializeField, Min(0f)] float Frequency;

		Dictionary<long,float> HurtboxCD = new Dictionary<long,float>();
		ColliderState _state;
		ColliderState State {
			get => _state;
			set {
				_state = value;
				Collider.enabled = _state == ColliderState.Open;
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
		public void stopCheckingCollision() => State = ColliderState.Closed;

		void OnTriggerStay2D(Collider2D other) {
			if (State != ColliderState.Closed) {
				Hurtbox hurtbox = other.gameObject.GetComponent<Hurtbox>();

				if (hurtbox != null && CanCollide(hurtbox) && TimedOut(hurtbox.ID)) {
					Debug.Log("HIP");
					hurtbox.ApplyHit(generateHit(other));
					Refresh(hurtbox.ID);
				}
			}
		}

		protected virtual bool CanCollide(Hurtbox hurtbox) => true;
		bool TimedOut(long hurtboxID) => !HurtboxCD.ContainsKey(hurtboxID) 
			|| (Frequency > 0 && HurtboxCD[hurtboxID] + Frequency < Time.time);
		void Refresh(long hurtboxID) => HurtboxCD[hurtboxID] = Time.time;

		public abstract IHit generateHit(Collider2D collision);
	}
}