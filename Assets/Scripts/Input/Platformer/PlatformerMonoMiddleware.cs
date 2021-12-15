using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Input.Core {
	public abstract class PlatformerMonoMiddleware : MonoMiddleware {
		public Optional<PlatformerInputProvider> DefaultProvider;

		public virtual void Awake() {
			if (DefaultProvider.Enabled) Attach(DefaultProvider.Value);
		}
	}
}