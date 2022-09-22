using UnityEngine;

namespace Thuleanx.Engine {
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsObject : MonoBehaviour {
		Rigidbody2D Body;
		public Collider2D Collider {get; private set; }
		public Vector2 Velocity { get => Body.velocity; private set => Body.velocity = value; }
		public float MaxAccel = .2f, MaxDecel = .2f;
		public float Mass => Body.mass;

		void Awake() {
			Body = GetComponent<Rigidbody2D>();
			Collider = GetComponent<Collider2D>();
		}

		public void SetType(RigidbodyType2D type) => Body.bodyType = type;
		public void Stop() => Velocity = Vector2.zero;
		public void SetPosition(Vector2 pos) => transform.position = pos;
		public void Knockback(Vector2 Force, ForceMode2D mode = ForceMode2D.Impulse) => Body.AddForce(Force, mode);
		public void SetVelocityOverride(Vector2 newVelocity) => Velocity = newVelocity;
		public void AccelerateTowards(Vector2 targetVelocity) {
			var velocity = Body.velocity;
			var deltaV = targetVelocity - velocity;
			var accel = deltaV; // / Time.deltaTime
			var limit = Vector2.Dot(deltaV, velocity) > 0f ?  MaxAccel: MaxDecel;
			var force = Body.mass * Vector2.ClampMagnitude(accel, limit);
			Body.AddForce(force, ForceMode2D.Force);
		}
	}
}