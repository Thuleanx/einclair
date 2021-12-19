using UnityEngine;
using UnityEngine.UI;

using Thuleanx.AI.Core;

namespace Thuleanx.UI.Core {
	public class BashcooldownTracker : MonoBehaviour {
		public Slider Slider;

		void Update() {
			if (Slider && (Context.ReferenceManager.Player is Hammerdude)) {
				Hammerdude dude = Context.ReferenceManager.Player as Hammerdude;
				if (!dude.BashCooldown) 	Slider.value = 1f;
				else 						Slider.value = 1 - dude.BashCooldown.TimeLeft / dude.BashCooldown.Duration;
			}
		}
	}
}