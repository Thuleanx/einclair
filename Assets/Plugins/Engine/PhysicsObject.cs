using UnityEngine;

namespace Thuleanx.Engine {
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsObject : MonoBehaviour {
		public Rigidbody2D Body {get; private set; }
		public Collider2D Collider {get; private set; }

		public Vector2 Velocity { get => Body.velocity; set => Body.velocity = value; }

		void Awake() {
			Body = GetComponent<Rigidbody2D>();
			Collider = GetComponent<Collider2D>();
		}

		// void FixedUpdate() {
		// 	Position = Body.position;

		// 	Vector2 Move = Velocity * Time.fixedDeltaTime;
		// 	Body.MovePosition((Vector2) Body.position + Move);
		// }

		public void Stop() => Velocity = Vector2.zero;
		public void Translate(Vector2 Displacement) => Body.MovePosition(Body.position + Displacement);
		public void SetPosition(Vector2 pos) => transform.position = pos;

		public void SetVelocityX(float vx) => Velocity = new Vector2(vx, Velocity.y);
		public void SetVelocityY(float vy) => Velocity = new Vector2(Velocity.x, vy);
	}
}