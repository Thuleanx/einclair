using UnityEngine;

namespace Thuleanx.Input {
	public interface InputMiddleware {
		InputState Process(InputState state);
		void Review(InputFeedback feedback);

		void Detach(InputProvider provider);
		void Attach(InputProvider provider);
	}
}