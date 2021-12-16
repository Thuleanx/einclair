using UnityEngine;

using Thuleanx.Utils;
using Thuleanx.Input.Core;
using Thuleanx.Combat;

namespace Thuleanx.AI.Core {
	public class BasicAI : LivePlatformerAI {
		protected PlatformerInputState InputState;

		public override void Update() {
			InputState = Provider.GetState() as PlatformerInputState;
			base.Update();
		}

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(1,0);
			StateMachine.SetCallbackUpdate(0, NormalUpdate);
			StateMachine.SetCallbackEnd(0, NormalExit);
		}

		public override void ObjectSetup() {
			base.ObjectSetup();
			if (Provider == null) Provider = ScriptableObject.CreateInstance<PlatformerInputProvider>();
		}

		#region Normal
		public virtual int NormalUpdate() {
			Vector2 Movement = InputState.Movement;

			// Platform code
			_isOnPlatform = PlatformCheck();
			transform.parent = _platform;

			// Horizontal
			{
				// // Turn around for free
				// if (Movement.x != 0 && Mathf.Sign(Body.Velocity.x) != Movement.x)
				// 	Body.SetVelocityX(-Body.Velocity.x);

				float current = Body.Velocity.x;
				float intention = Movement.x * baseMovementSpeed;

				Body.AccelerateTowards(new Vector2(intention, Body.Velocity.y));

				if (Movement.x < 0 && IsRightFacing) Flip();
				else if (Movement.x > 0 && !IsRightFacing) Flip();

				// Prevent Edge Falloff
				if (LedgeAhead(IsRightFacing))
					Body.AccelerateTowards(Vector2.zero);
			}

			return -1;
		}
		protected void NormalExit() {
			_isOnPlatform = false;
			transform.parent = _platform;
		}
		#endregion
	}
}