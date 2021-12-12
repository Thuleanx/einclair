using UnityEngine;

namespace Thuleanx.Effects.Particle {
	[ExecuteAlways]
	public class ParticleFaceToScale : MonoBehaviour {
		void OnWillRenderObject() {
			if (transform.lossyScale.x < 0)
				transform.localScale = new Vector3(
					transform.localScale.x * -1, 
					transform.localScale.y, 
					transform.localScale.z
				);
		}
	}
}