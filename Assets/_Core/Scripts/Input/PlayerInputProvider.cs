using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	[CreateAssetMenu(fileName = "PIP", menuName = "~Einclair/PlayerInputProvider", order = 0)]
	public class PlayerInputProvider : ScriptableObject, InputProvider {
		public PlayerInputFeedback Feedback = new PlayerInputFeedback();


		List<KeyValuePair<int, InputMiddleware>> Middlewares = new List<KeyValuePair<int, InputMiddleware>>();

		public InputState GetState() {
			PlayerInputState InputState = new PlayerInputState();
			foreach (var kvp in Middlewares)
				InputState = kvp.Value.Process(InputState) as PlayerInputState;
			return InputState;
		}

		public void ProcessFeedback() {
			foreach (var kvp in Middlewares)
				kvp.Value.Review(Feedback);
			Feedback.Reset();
		}

		public void AddMiddleware(InputMiddleware middleware, int priority) {
			Debug.Log(Middlewares.Count);
			Middlewares.Add(new KeyValuePair<int,InputMiddleware>(priority, middleware));
			Middlewares.Sort((kvp1, kvp2) => {
				return kvp1.Key - kvp2.Key;
			});
		}
		public void RemoveMiddleware(InputMiddleware middleware, int priority) {
			KeyValuePair<int,InputMiddleware> kvp = new KeyValuePair<int, InputMiddleware>(priority, middleware);
			Middlewares.Remove(kvp);
		}
	}
}