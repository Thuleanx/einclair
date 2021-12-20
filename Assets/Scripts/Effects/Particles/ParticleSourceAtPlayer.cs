using UnityEngine;
using System.Collections.Generic;


namespace Thuleanx.Effects.Particles.Core {
	[RequireComponent(typeof(ParticleSystem))]
	public class ParticleSourceAtPlayer : MonoBehaviour {
		public ParticleSystem System {get; private set; }
		public Vector3 Offset;
		void Awake() {
			System = GetComponent<ParticleSystem>();
		}
		void Update() {
			var shape = System.shape;
			shape.position = (Context.ReferenceManager.Player.transform.position - transform.position) + Offset;
		}
	}
}