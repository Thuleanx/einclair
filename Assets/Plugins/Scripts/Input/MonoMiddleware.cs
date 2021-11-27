using UnityEngine;

namespace Thuleanx.Input {
	public abstract class MonoMiddleware : MonoBehaviour, InputMiddleware {
		public void Attach(InputProvider provider) => provider.AddMiddleware(this);
		public void Detach(InputProvider provider) => provider.RemoveMiddleware(this);
		public abstract InputState Process(InputState state);
		public abstract void Review(InputFeedback feedback);
	}
}