using UnityEngine;
using Cinemachine;

using Thuleanx.Effects.Particles;
using Thuleanx.Utils;

namespace Thuleanx.Combat.Core {
	[RequireComponent(typeof(ParticleCombo))]
	public class Explosion : MonoBehaviour {
		public PlatformerHitbox Hitbox;
		public float DamageDuration = .2f;
		Timer _damaging;

		void OnEnable() {
			Hitbox?.startCheckingCollision();
			_damaging = new Timer(DamageDuration);
			_damaging.Start();
			GetComponent<ParticleCombo>().Activate();
			GetComponent<CinemachineImpulseSource>()?.GenerateImpulse();
		}
		void Update() {
			if (!_damaging && Hitbox.Active) {
				Hitbox.stopCheckingCollision();
			}
		}
	}
}