using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Thuleanx.Interaction {
	public class UnityEventReceiver : MonoBehaviour, INotificationReceiver {
        public UnityEvent Eve;
		public SignalAsset[] signalAssets;

		public void OnNotify(Playable origin, INotification notification, object context) {
			if (notification is SignalEmitter emitter) {
				var matches = signalAssets.Where(x => ReferenceEquals(x, emitter.asset));
				if (matches.Count() > 0) 
					Eve?.Invoke();
			}
		}
	}
}