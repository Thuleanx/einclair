using UnityEngine;

namespace Thuleanx.Input {
	public interface InputProvider {
		InputState GetState();
		void ProcessFeedback();

		void AddMiddleware(InputMiddleware middleware);
		void RemoveMiddleware(InputMiddleware middleware);
	}
}