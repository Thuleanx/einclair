using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class PlatformerInputProvider : ScriptableObject, InputProvider {
		public InputFeedback Feedback;
		protected List<KeyValuePair<int, InputMiddleware>> Middlewares = new List<KeyValuePair<int, InputMiddleware>>();

		public virtual InputState GetState() {
			InputState inputState = BlankState();
			foreach (var kvp in Middlewares)
				inputState = kvp.Value.Process(inputState);
			if (Feedback == null) Feedback = inputState.GetFeedbackBlueprint();
			return inputState;
		}

		public virtual void ProcessFeedback() {
			foreach (var kvp in Middlewares)
				kvp.Value.Review(Feedback);
			Feedback.Reset();
		}

		public void AddMiddleware(InputMiddleware middleware, int priority) {
			Middlewares.Add(new KeyValuePair<int,InputMiddleware>(priority, middleware));
			Middlewares.Sort((kvp1, kvp2) => {
				return kvp1.Key - kvp2.Key;
			});
		}
		public void RemoveMiddleware(InputMiddleware middleware, int priority) {
			KeyValuePair<int,InputMiddleware> kvp = new KeyValuePair<int, InputMiddleware>(priority, middleware);
			Middlewares.Remove(kvp);
		}

        public virtual InputState BlankState() => new PlatformerInputState();
    }
}