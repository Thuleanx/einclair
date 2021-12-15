using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Input {
	public abstract class MonoMiddleware : MonoBehaviour, InputMiddleware {
		public void Attach(InputProvider provider) => provider.AddMiddleware(this, GetPriority());
		public void Detach(InputProvider provider) => provider.RemoveMiddleware(this, GetPriority());
		public abstract int GetPriority();
		public abstract InputState Process(InputState state);
		public abstract void Review(InputFeedback feedback);
	}
}