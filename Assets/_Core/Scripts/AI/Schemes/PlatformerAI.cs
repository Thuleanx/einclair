using UnityEngine;

using Thuleanx.Input.Core;
using Draggable;

namespace Thuleanx.AI.Core {
	public class PlatformerAI : Agent {
		#region Components
		public Animator Anim {get; private set; }
		#endregion

		[Header("General")]
		[SerializeField] protected LayerMask groundLayer;
		[SerializeField] protected LayerMask platformLayer;
		public PlatformerInputProvider Provider;

		[Header("Movement")]
		[SerializeField] protected bool defaultLeftFacing;
		[SerializeField] protected float baseMovementSpeed;
		[SerializeField] protected float groundAccelLambda;
		[SerializeField] protected float fallMaxVelocity;
		[DraggablePoint(true)] public Vector3 LedgeRightAnchor;

		protected bool _isFacingRight = true;
		protected bool _isOnPlatform = false;
		protected Transform _platform = null;

		public override void ObjectSetup() {
			Anim = GetComponent<Animator>();
		}

		public void Flip() {
			_isFacingRight = !_isFacingRight;
			Vector3 transformScale = transform.localScale;
			transformScale.x *= -1;
			transform.localScale = transformScale;
		}

		#region Utils
		public bool OnGround()
			=> (bool) Physics2D.CircleCast(Body.Collider.bounds.center, Body.Collider.bounds.size.y / 2, 
				Vector2.down, .1f, groundLayer);
		public bool PlatformCheck() {
			RaycastHit2D hit = Physics2D.CircleCast(Body.Collider.bounds.center, Body.Collider.bounds.size.y / 2,  
				Vector2.down, .1f, platformLayer);
			if (hit && _isOnPlatform) _isOnPlatform = true;
			if (hit) 	this._platform = hit.collider.gameObject.transform;
			else 		this._platform = null;
			return hit;
		}
		public bool LedgeAhead(bool right) {
			Vector2 pos = (Vector2) transform.position + (new Vector2(LedgeRightAnchor.x * (right ? 1 : -1), LedgeRightAnchor.y));
			return !Physics2D.OverlapCircle(pos, .1f, groundLayer | platformLayer);
		}
		public void ForceDirection(bool right) {
			if (_isFacingRight ^ right)
				Flip();
		}

		public bool IsRightFacing => _isFacingRight ^ defaultLeftFacing;

		protected void SetBodyDynamic() {
			Body.Body.bodyType = RigidbodyType2D.Dynamic;
			// Prevent Jitter
			Body.Velocity = Vector2.zero;
			Body.SetPositionY(Mathf.Round(Body.transform.position.y));
		}
		protected void SetBodyKinematic() {
			Body.Body.bodyType = RigidbodyType2D.Kinematic;
		}
		#endregion
	}
}