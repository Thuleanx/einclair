using UnityEngine;
using UnityEngine.UI;

using Thuleanx.AI.Core;
using Thuleanx.Utils;

using MarkupAttributes;
using NaughtyAttributes;

namespace Thuleanx.UI.Core {
	public class HealthBar : MonoBehaviour {
		public Optional<LivePlatformerAI> Tracking;

		[Box("Health bar")]
		public Slider LiveSlider;
		public Slider Catchup;
		public float CatchupRateAlpha = 18f;

		[Box("Crack")]
		public Material Mat;
		[MinMaxSlider(0,1)]
		public Vector2 CrackRange;
		public AnimationCurve Curve;

		void Start() {
			if (!Tracking.Enabled) Tracking = new Optional<LivePlatformerAI>(Context.ReferenceManager.Player);
		}

		void Update() {
			float helth = (float) Tracking.Value.Health / Tracking.Value.MaxHealth;
			LiveSlider.value = helth;
			if (Catchup) Catchup.value = Calc.Damp(Catchup.value, helth, CatchupRateAlpha, Time.deltaTime);
			Mat?.SetFloat("_Crack", Mathf.Lerp(CrackRange.y, CrackRange.x, Curve.Evaluate(helth)));
		}
	}
}