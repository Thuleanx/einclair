using UnityEngine;

using Thuleanx.Utils;

namespace Thuleanx.Engine {
	public class ExpireAfterDuration : MonoBehaviour {
		public float Duration;
		Timer _Timer;

		void Awake() {
			_Timer = new Timer(Duration);
		}

		void OnEnable() {
			_Timer.Start();
		}

		void Update() {
			if (!_Timer) General.TryDispose(gameObject);
		}
	}
}