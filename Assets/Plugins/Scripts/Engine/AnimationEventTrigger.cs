using UnityEngine;
using UnityEngine.Events;

namespace Thuleanx.Engine.Core {
	public class AnimationEventTrigger : MonoBehaviour {
		[SerializeField]
		UnityEvent[] EventTriggers;

		void _Animation_Activate(int index) {
			if (EventTriggers != null && index >= 0 && index < EventTriggers.Length)
				EventTriggers[index]?.Invoke();
		}
	}
}