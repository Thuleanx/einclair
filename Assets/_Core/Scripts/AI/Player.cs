using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

using Thuleanx.Engine.Core;
using Thuleanx.Utils;
using Thuleanx.Input.Core;

using Thuleanx.Manager.Core;

namespace Thuleanx.AI.Core {

	enum PlayerAnimationState {
		Grounded = 0,
		Airborne = 1
	}

	[RequireComponent(typeof(Animator))]
	public class Player : Agent {
		public enum PlayerState {
			Normal = 0, 
			Climb = 1, 
			Dead = 2
		}

		#region Components
		public Animator Anim {get; private set;}
		#endregion

		[Header("General")]
		[SerializeField] LayerMask groundLayer;
		[SerializeField] LayerMask platformLayer;
		PlayerInputProvider Provider { get => GlobalReferences.PlayerInputProvider; }

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(PlayerState)).Length, (int) PlayerState.Normal);
			StateMachine.SetCallbackUpdate((int) PlayerState.Normal, NormalUpdate);
			StateMachine.SetCallbackEnd((int) PlayerState.Normal, NormalExit);

			StateMachine.SetCallbackUpdate((int) PlayerState.Climb, ClimbUpdate);
			StateMachine.SetCallbackTransition((int) PlayerState.Climb, ClimbTransition);
			StateMachine.SetCallbackBegin((int) PlayerState.Climb, ClimbEnter);
			StateMachine.SetCallbackEnd((int) PlayerState.Climb, ClimbExit);
		}

		public override void ObjectSetup() {
			Anim = GetComponent<Animator>();
			_AnimState = PlayerAnimationState.Grounded;
			_jumpCoyote = new Timer(coyoteTime);
			_variableJump = new Timer(varJumpTime);
		}

		public override void Update() {
			InputState = Provider.GetState() as PlayerInputState;
			base.Update();
			AnimationUpdate();
		}

		public void AnimationUpdate() {
			Anim.SetFloat("VelocityX", Body.Velocity.x);
			Anim.SetFloat("VelocityY", Body.Velocity.y);
		}

		#region Object Scope

		PlayerInputState InputState;

		bool _isFacingRight = true;
		bool _isOnPlatform = false;

		Transform _platform = null;

		Timer _jumpCoyote;
		Timer _variableJump;

		PlayerAnimationState _AnimState {
			get => (PlayerAnimationState) Anim.GetInteger("State"); 
			set {
				Anim.SetInteger("State", (int) value);
			}
		}

		#endregion

		#region Normal
		[Header("Movement")]
		[SerializeField] bool defaultLeftFacing;
		[SerializeField] float baseMovementSpeed;
		[SerializeField] float groundAccelLambda;
		[SerializeField] float fallMaxVelocity;

		int NormalUpdate() {
			Vector2 Movement = InputState.Movement;

			// Platform code
			_isOnPlatform = PlatformCheck();
			transform.parent = _platform;

			// Horizontal
			{
				// Turn around for free
				if (Movement.x != 0 && Mathf.Sign(Body.Velocity.x) != Movement.x)
					Body.SetVelocityX(-Body.Velocity.x);

				float current = Body.Velocity.x;
				float intention = Movement.x * baseMovementSpeed;

				Body.SetVelocityX(Calc.Damp(current, intention, groundAccelLambda, Time.deltaTime));

				if (Movement.x < 0 && (_isFacingRight ^ defaultLeftFacing)) Flip();
				else if (Movement.x > 0 && (!_isFacingRight ^ defaultLeftFacing)) Flip();
			}

			{
				if (OnGround() || _isOnPlatform) {
					_jumpCoyote.Start();
					_AnimState = PlayerAnimationState.Grounded;
				} else {
					if (Body.Velocity.y < -fallMaxVelocity)
						Body.SetVelocityY(-fallMaxVelocity);
					_AnimState = PlayerAnimationState.Airborne;
				}

				if (InputState.Jump && _jumpCoyote)
					Jump();
				if (InputState.JumpReleased && _variableJump)
					VarJump();

			}
			return -1;
		}
		void NormalExit() {
			_isOnPlatform = false;
			transform.parent = _platform;
		}
		#endregion 

		#region Jump

		[Header("Jump")]
		[SerializeField] float airMult;
		[SerializeField, MinMaxSlider(0, 10)] Vector2 jumpHeight;
		[SerializeField] float coyoteTime = .1f;
		[SerializeField] float varJumpTime = 1f;

		public float MaxJumpVelocity => Mathf.Sqrt(jumpHeight.y * 2 * App.Gravity);
		public float MinJumpVelocity => Mathf.Sqrt(jumpHeight.x * 2 * App.Gravity);


		void Jump() {
			_jumpCoyote.Stop();
			_variableJump.Start();

			Body.SetVelocityY(MaxJumpVelocity);
			Provider.Feedback.JumpExecuted = true;
		}

		void VarJump() {
			_variableJump.Stop();
			Body.SetVelocityY(Mathf.Min(MinJumpVelocity, Body.Velocity.y));
			Provider.Feedback.JumpReleaseExecuted = true;
		}

		#endregion

		#region Climb
		[Header("Climb")]
		public float ClimbSpeed = 2f;

		Ladder _ladder = null;

		void ClimbEnter() {
			// Check all colliding objects for a ladder
			List<Collider2D> results = new List<Collider2D>();
			ContactFilter2D contactFilter = new ContactFilter2D();
			contactFilter.SetLayerMask(Calc.GetPhysicsLayerMask(gameObject.layer));
			Physics2D.OverlapCollider(Body.Collider,contactFilter, results);

			foreach (Collider2D res in results) {
				Ladder lad = res.GetComponent<Ladder>();
				if (lad) _ladder = lad;
			}

			Body.Body.bodyType = RigidbodyType2D.Kinematic;
		}
		int ClimbTransition() {
			if (InputState.Movement.y > 0 && transform.position.y >= _ladder.Top.y)
				return (int) PlayerState.Normal;
			if (InputState.Movement.y < 0 && transform.position.y <= _ladder.Bot.y)
				return (int) PlayerState.Normal;
			return -1;
		}
		int ClimbUpdate() {
			Vector2 Movement = InputState.Movement;
			Body.SetPositionX(_ladder.transform.position.x);
			Body.SetVelocityY(Movement.y * ClimbSpeed);
			return -1;
		}
		void ClimbExit() {
			Body.Body.bodyType = RigidbodyType2D.Dynamic;
			// Prevent Jitter
			Body.Velocity = Vector2.zero;
			Body.SetPositionY(Mathf.Round(Body.transform.position.y));
		}
		#endregion

		#region Utils
		public void Flip() {
			_isFacingRight = !_isFacingRight;
			Vector3 transformScale = transform.localScale;
			transformScale.x *= -1;
			transform.localScale = transformScale;
		}
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
		#endregion
	}
}