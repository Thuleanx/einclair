using UnityEngine;

using Thuleanx.Utils;
using Thuleanx.Input.Core;

namespace Thuleanx.AI.Core {
	public class BasicAI : PlatformerAI {
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
			Provider = ScriptableObject.CreateInstance<PlatformerInputProvider>();
		}

		#region Normal
		protected int NormalUpdate() {
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

				// Prevent Edge Falloff
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