using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Thuleanx.Utils;

namespace Thuleanx.Controls.Core {

	[DisallowMultipleComponent]
	public class InputManager : MonoBehaviour {
		public static float InputBufferTime = .2f;

		public static InputManager Instance;
		
		[HideInInspector]
		public Vector2 MouseScreenPos = Vector2.zero;
		public Vector2 MouseWorldPos {
			get {
				if (Camera.main != null)
					return Camera.main.ScreenToWorldPoint(MouseScreenPos);
				return Vector2.zero;
			}
		}

		[HideInInspector] public float Movement;

		#region Timers
		public Timer Jump;
		public Timer JumpReleased;
		#endregion

		void Awake() {
			Instance = this;

			Jump = new Timer(InputBufferTime);
			JumpReleased = new Timer(InputBufferTime);
		}
		
		public void OnMoveInput(InputAction.CallbackContext context)
			=> Movement = context.ReadValue<float>();
		public void OnJumpInput(InputAction.CallbackContext context ) {
			if (context.started) Jump.Start();
			if (context.canceled) JumpReleased.Start();
		}
	}
}
