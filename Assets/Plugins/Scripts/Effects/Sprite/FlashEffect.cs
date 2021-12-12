using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Effects.Sprite {
	[RequireComponent(typeof(SpriteRenderer))]
	public class FlashEffect : MonoBehaviour {
		[SerializeField] Material FlashMaterial;
		[SerializeField] float flashDuration;
		// [SerializeField, Min(1f)] float FlashPerMinute = 1f;

		Material DefaultMaterial;
		Timer Flashing;

		public SpriteRenderer Sprite {get; private set; }

		void Awake() {
			Sprite = GetComponent<SpriteRenderer>();
			Flashing = new Timer(flashDuration);
			DefaultMaterial = Sprite.material;
		}

		public void Flash() => Flashing.Start();

		void Update() {
			if (Flashing) {
				Sprite.material = FlashMaterial;
				// float timeSinceStart = Flashing.Duration - Flashing.TimeLeft;
				// float flashInterval = 60f / FlashPerMinute / 2;
				// Sprite.material = (((int) (timeSinceStart / flashInterval)) & 1) == 0 ? FlashMaterial : DefaultMaterial;
			} else {
				Sprite.material = DefaultMaterial;
			}
		}
	}
}