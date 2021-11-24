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

	public class Player : Agent {
		[Header("General")]
		[SerializeField] LayerMask groundLayer;

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
		Timer _jumpCoyote;
		Timer _variableJump;

		#endregion

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(PlayerState)).Length, (int) PlayerState.Normal);
			StateMachine.SetCallbacks((int) PlayerState.Normal, NormalUpdate, null, null, null, null);

		}

		public override void ObjectSetup() {
			_jumpCoyote = new Timer(coyoteTime);
			_variableJump = new Timer(varJumpTime);
		}

		int NormalUpdate() {
			float Movement = InputManager.Instance.Movement;

			// Apply gravity
			// Body.Velocity += Time.deltaTime * Physics2D.gravity;

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
				if (OnGround()) _jumpCoyote.Start();
				else {
					if (Body.Velocity.y < -fallMaxVelocity)
						Body.SetVelocityY(-fallMaxVelocity);
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

		public void Flip() {
			_isFacingRight = !_isFacingRight;
			Vector3 transformScale = transform.localScale;
			transformScale.x *= -1;
			transform.localScale = transformScale;
		}

		public bool OnGround()
			=> (bool) Physics2D.CircleCast(Body.Collider.bounds.center, Body.Collider.bounds.size.y/2, Vector2.down, .1f, groundLayer);
	}
}