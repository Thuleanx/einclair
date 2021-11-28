using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Thuleanx.Interaction.Core {
	public class PlayerStateReceiver : MonoBehaviour, INotificationReceiver {
		public SignalAsset[] signalAssets;

		public void OnNotify(Playable origin, INotification notification, object context) {
			if (notification is PlayerStateEmitter emitter) {
				var matches = signalAssets.Where(x => ReferenceEquals(x, emitter.asset));
				if (matches.Count() > 0)
					emitter.Execute();
			}
		}
	}
}