using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Yarn.Unity;

namespace Thuleanx.Effects.Particles {
	public class ParticleCombo : MonoBehaviour {
		List<ParticleSystem> burstSystems;

		void Awake() {
			RegisterSystem();
		}
		public void RegisterSystem() {
			burstSystems = new List<ParticleSystem>();
			foreach (Transform child in transform) {
				ParticleSystem system = child.GetComponent<ParticleSystem>();
				if (system != null) burstSystems.Add(system);
			}
		}
		public void Activate() {
			if (burstSystems == null) RegisterSystem();

			foreach (ParticleSystem system in burstSystems) {
				system.Stop();
				system.Clear();
				system.Play();
				Debug.Log("HI");
			}
		}
		public void Stop() {
			if (burstSystems == null) RegisterSystem();

			foreach (ParticleSystem system in burstSystems) {
				system.Stop();
				system.Clear();
			}
		}
	}
}
