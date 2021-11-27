using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	[CreateAssetMenu(fileName = "PIP", menuName = "~Einclair/PlayerInputProvider", order = 0)]
	public class PlayerInputProvider : ScriptableObject, InputProvider {
		[SerializeField] List<InputMiddleware> Middlewares = new List<InputMiddleware>();
		public PlayerInputFeedback Feedback = new PlayerInputFeedback();

		public InputState GetState() {
			PlayerInputState InputState = new PlayerInputState();
			foreach (InputMiddleware middleware in Middlewares)
				InputState = middleware.Process(InputState) as PlayerInputState;
			return InputState;
		}

		public void ProcessFeedback() {
			foreach (InputMiddleware middle in Middlewares)
				middle.Review(Feedback);
			Feedback.Reset();
		}

		public void AddMiddleware(InputMiddleware middleware) => Middlewares.Add(middleware);
		public void RemoveMiddleware(InputMiddleware middleware) => Middlewares.Remove(middleware);
	}
}