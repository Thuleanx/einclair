using UnityEngine;

namespace Thuleanx.Input {
	public interface InputProvider {
		InputState GetState();
		void ProcessFeedback();

		void AddMiddleware(InputMiddleware middleware, int priority);
		void RemoveMiddleware(InputMiddleware middleware, int priority);
	}
}