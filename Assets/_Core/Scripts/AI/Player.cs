using UnityEngine;
using System;
using NaughtyAttributes;

using Thuleanx.Controls.Core;
using Thuleanx.Utils;

namespace Thuleanx.AI._Core {
	enum PlayerState {
		Normal = 0, 
		Dead = 1
	}

	enum PlayerAnimationState {
		Grounded = 0,
		Airborne = 1
	}

	[RequireComponent(typeof(Animator))]
	public class Player : Agent {
		#region Components
		public Animator Anim {get; private set;}
		#endregion

		[Header("General")]
		[SerializeField] LayerMask groundLayer;
		[SerializeField] LayerMask platformLayer;


		[Header("Movement")]
		[SerializeField] bool defaultLeftFacing;
		[SerializeField] float baseMovementSpeed;
		[SerializeField] float groundAccelLambda;
		[SerializeField] float fallMaxVelocity;

		[Header("Jump")]
		[SerializeField] float airMult;
		[SerializeField, MinMaxSlider(0, 10)] Vector2 jumpHeight;
		[SerializeField] float coyoteTime = .1f;
		[SerializeField] float varJumpTime = 1f;

		public float MaxJumpVelocity => Mathf.Sqrt(jumpHeight.y * 2 * App.Gravity);
		public float MinJumpVelocity => Mathf.Sqrt(jumpHeight.x * 2 * App.Gravity);


		#region Object Scope

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

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(PlayerState)).Length, (int) PlayerState.Normal);
			StateMachine.SetCallbackUpdate((int) PlayerState.Normal, NormalUpdate);
		}

		public override void ObjectSetup() {
			Anim = GetComponent<Animator>();
			_AnimState = PlayerAnimationState.Grounded;
			_jumpCoyote = new Timer(coyoteTime);
			_variableJump = new Timer(varJumpTime);
		}

		public override void Update() {
			base.Update();
			AnimationUpdate();
		}

		public void AnimationUpdate() {
			Anim.SetFloat("VelocityX", Body.Velocity.x);
			Anim.SetFloat("VelocityY", Body.Velocity.y);
		}

		int NormalUpdate() {
			float Movement = InputManager.Instance.Movement;

			// Platform code
			_isOnPlatform = PlatformCheck();
			transform.parent = _platform;

			// Horizontal
			{
				// Turn around for free
				if (Movement != 0 && Mathf.Sign(Body.Velocity.x) != Movement)
					Body.SetVelocityX(-Body.Velocity.x);

				float current = Body.Velocity.x;
				float intention = InputManager.Instance.Movement * baseMovementSpeed;

				Body.SetVelocityX(Calc.Damp(current, intention, groundAccelLambda, Time.deltaTime));

				if (Movement < 0 && (_isFacingRight ^ defaultLeftFacing)) Flip();
				else if (Movement > 0 && (!_isFacingRight ^ defaultLeftFacing)) Flip();
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

				if (InputManager.Instance.Jump && _jumpCoyote)
					Jump();
				if (InputManager.Instance.JumpReleased && _variableJump)
					VarJump();

			}
			return -1;
		}

		#region Jump

		void Jump() {
			InputManager.Instance.Jump.Stop();
			_jumpCoyote.Stop();
			_variableJump.Start();

			Body.SetVelocityY(MaxJumpVelocity);
		}

		void VarJump() {
			InputManager.Instance.JumpReleased.Stop();
			_variableJump.Stop();
			Body.SetVelocityY(Mathf.Min(MinJumpVelocity, Body.Velocity.y));
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