using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

using Thuleanx.Engine;
using Thuleanx.Utils;
using Thuleanx.Input;
using Thuleanx.Input.Core;

using Thuleanx.Manager.Core;

using Thuleanx.Combat;
using Thuleanx.Optimization;
using Thuleanx.Combat.Core;

using MarkupAttributes;

namespace Thuleanx.AI.Core {

	enum PlayerAnimationState {
		Grounded = 0,
		Airborne = 1,
		Climb = 2,
		Lock = 3
	}

	[RequireComponent(typeof(Animator))]
	public class Player : LivePlatformerAI, InputMiddleware {
		public enum PlayerState {
			Normal = 0, 
			Climb = 1, 
			Lock = 2,
			Dead = 3
		}

		public new PlayerInputProvider Provider { get => GlobalReferences.PlayerInputProvider; }

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(PlayerState)).Length, (int) PlayerState.Normal);
			// NORMAL
			StateMachine.SetCallbackUpdate((int) PlayerState.Normal, NormalUpdate);
			StateMachine.SetCallbackEnd((int) PlayerState.Normal, NormalExit);
			// CLIMB
			StateMachine.SetCallbackUpdate((int) PlayerState.Climb, ClimbUpdate);
			StateMachine.SetCallbackTransition((int) PlayerState.Climb, ClimbTransition);
			StateMachine.SetCallbackBegin((int) PlayerState.Climb, ClimbEnter);
			StateMachine.SetCallbackEnd((int) PlayerState.Climb, ClimbExit);
			// LOCK 
			StateMachine.SetCallbackBegin((int) PlayerState.Lock, LockEnter);
			StateMachine.SetCallbackTransition((int) PlayerState.Lock, LockTransition);
			StateMachine.SetCallbackEnd((int) PlayerState.Lock, LockExit);
		}

		public override void ObjectSetup() {
			base.ObjectSetup();
			_AnimState = PlayerAnimationState.Grounded;
			_jumpCoyote = new Timer(coyoteTime);
			_variableJump = new Timer(varJumpTime);
			_originalParent = gameObject.transform.parent;
			Attach(Provider);
		}
		public override void Update() {
			InputState = Provider.GetState() as PlayerInputState;
			base.Update();
		}
		void LateUpdate() {
			AnimationUpdate();
		}
		public void AnimationUpdate() {
			Anim.SetFloat("VelocityX", Body.Velocity.x);
			Anim.SetFloat("VelocityY", Body.Velocity.y);
		}

		#region Object Scope

		PlayerInputState InputState;
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
		Transform _originalParent;
		int NormalUpdate() {
			Vector2 Movement = InputState.Movement;

			// Platform code
			_isOnPlatform = PlatformCheck();
			transform.parent = _platform ? _platform : _originalParent;

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

			if (InputState.Attack) {
				// Attack
				StartAttack();
			}

			return -1;
		}
		void NormalExit() {
			_isOnPlatform = false;
			transform.parent = _platform;
		}
		#endregion 
		#region Jump

		[Box("Air")]
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
			(Provider.Feedback as PlayerInputFeedback).JumpExecuted = true;
		}

		void VarJump() {
			_variableJump.Stop();
			Body.SetVelocityY(Mathf.Min(MinJumpVelocity, Body.Velocity.y));
			(Provider.Feedback as PlayerInputFeedback).JumpReleaseExecuted = true;
		}

		#endregion
		#region Climb
		[Header("Climb")]
		public float ClimbSpeed = 2f;
		public float ClimbXOffset = 0;
		public bool ClimbForceFaceRight = true;
		public string ClimbRecoveryTrigger = "ClimbRecovery";

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
			_AnimState = PlayerAnimationState.Climb;

			SetBodyKinematic();
			if (ClimbForceFaceRight && !_isFacingRight)
				Flip();
		}
		int ClimbTransition() {
			if ((InputState.Movement.y > 0 && transform.position.y >= _ladder.Top.y) ||
				(InputState.Movement.y < 0 && transform.position.y <= _ladder.Bot.y)
			) {
				Lock(ClimbRecoveryTrigger, (animationFinish) => {
					return animationFinish ? (int) PlayerState.Normal : -1;
				});
			}
			return -1;
		}
		int ClimbUpdate() {
			Vector2 Movement = InputState.Movement;
			Body.SetPositionX(_ladder.transform.position.x + ClimbXOffset);
			Body.SetVelocityY(Movement.y * ClimbSpeed);
			return -1;
		}
		void ClimbExit() {
			SetBodyDynamic();
			// Prevent Jitter
			Body.Velocity = Vector2.zero;
			Body.SetPositionY(Mathf.Round(Body.transform.position.y));
		}
		#endregion
		#region Lock
		bool _lockAnimFinished;
		Func<bool, int> _lockTransition = (fin)=>-1;

		void LockEnter() {
			_lockAnimFinished = false;
			// SetBodyKinematic();
			Body.Velocity = Vector2.zero;
			AnimationFinish.AddListener(LockAnimationEnds);
		}
		int LockTransition() => _lockTransition(_lockAnimFinished);
		void LockExit() {
			SetBodyDynamic();
			AnimationFinish.RemoveListener(LockAnimationEnds);
		}
		public void LockAnimationEnds() => _lockAnimFinished = true;

		public void Lock(string animationTrigger, Func<bool, int> LockTransition, bool setBodyKinematic = true) {
			// TODO: Deal with death state
			if (LockTransition != null && !IsLocked) {
				if (setBodyKinematic) SetBodyKinematic();
				_AnimState = PlayerAnimationState.Lock;
				Anim.SetTrigger(animationTrigger);
				_lockTransition = LockTransition;
				StateMachine.State = (int) PlayerState.Lock;
			} else Debug.Log("lock transition is null or character is still locked");
		}

		public bool IsLocked => StateMachine.State == (int) PlayerState.Lock;
		#endregion
		#region Utils
		UnityEvent AnimationFinish = new UnityEvent();
		public void FinishAnimation()=>AnimationFinish?.Invoke();

		public int GetPriority() => (int) MiddlewarePriority.PLAYER;

		public InputState Process(InputState state) {
			if (state is PlayerInputState) {
				PlayerInputState PIP = state as PlayerInputState;
				PIP.CanInteract = StateMachine.State == (int) PlayerState.Normal;
				return PIP;
			}
			return state;
		}

		public void Review(InputFeedback feedback) {}

		public void Detach(InputProvider provider) => provider.RemoveMiddleware(this, GetPriority());
		public void Attach(InputProvider provider) => provider.AddMiddleware(this, GetPriority());
		#endregion

		#region Combat
		[Box("Combat")]
		public string AttackTrigger;
		public GameObject Lantern;
		public BubblePool BulletPool;

		public void Attack() {
			if (BulletPool) {
				GameObject obj = BulletPool.Borrow(Lantern.transform.position, Quaternion.identity);
				obj.GetComponent<Bullet>()?.Init(InputState.TargetPosition - (Vector2) Lantern.transform.position);
			}
		}
		public void StartAttack() {
			Lock(AttackTrigger, (animationFinish) => {
				return animationFinish ? (int) PlayerState.Normal : -1;
			}, false);
			(Provider.Feedback as PlayerInputFeedback).AttackExecuted = true;
		}


		public string SpecialAttackTrigger;
		public BubblePool SpecialAbilityPool;
		public void StartSpecialAttack() {
			if (SpecialAbilityPool) {
				GameObject obj = SpecialAbilityPool.Borrow(Lantern.transform.position, Quaternion.identity);
			}
			Lock(SpecialAttackTrigger, (animationFinish) => {
				return animationFinish ? (int) PlayerState.Normal : -1;
			}, false);
		}
		#endregion
	}
}