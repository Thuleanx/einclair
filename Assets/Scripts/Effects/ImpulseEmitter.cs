using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Effects.Core {
	public class ImpulseEmitter : MonoBehaviour {
		public float Interval = 2f;
		Timer _CD;
		void Update() {
			if (!_CD) {
				PostProcessingUnit.Instance.StartShockwave(transform.position);
				_CD = new Timer(Interval);
				_CD.Start();
			}
		}
	}
}