using UnityEngine;

using Thuleanx.Effects.Particles;
using Thuleanx.AI.Core;

namespace Thuleanx.SceneManagement.Core {
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(ParticleCombo))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class Gate : MonoBehaviour {
		public int NumFrames = 10;
		int _frame = 0;
		bool _active;
		bool Active {
			get => _active;
			set {
				_active = value;
				Collider.enabled = value;
				Renderer.enabled = value;
				if (value) {
					Combo.Activate();
				}
				else Combo.Stop();
			}
		}

		public Collider2D Collider {get; private set; }
		public SpriteRenderer Renderer {get; private set; }
		public ParticleCombo Combo {get; private set; }

		void Awake() {
			Collider = GetComponent<Collider2D>();
			Combo = GetComponent<ParticleCombo>();
			Renderer = GetComponent<SpriteRenderer>();

			Active = false;
		}

		void Update() {
			if (++_frame == NumFrames) {
				bool enemyExist = FindObjectsOfType<LivePlatformerAI>().Length > 1;
				if (Active != enemyExist)
					Active = enemyExist;
				_frame = 0;
			}
		}
	}
}