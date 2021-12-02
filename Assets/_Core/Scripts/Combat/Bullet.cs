using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Utils;
using Thuleanx.Combat.Core;

namespace Thuleanx.Combat.Core {
	public class Bullet : PlatformerHitbox {
		public Rigidbody2D Body {get; private set; }

		public static int FRAMES_PER_CHECK = 10;
		int _frameSinceCheck;

		[Header("Bullet")]
		[SerializeField] float speed;

		public override void Awake() {
			Body = GetComponent<Rigidbody2D>();
		}

		public void Init(Vector2 dir) {
			Body.velocity = dir * speed;
		}

		public void Despawn() {
			gameObject.SetActive(false);
		}

		public void OnSpawnerDie() {
			Despawn();
		}

		void Update() {
			if (_frameSinceCheck++ >= FRAMES_PER_CHECK) {
				Vector2 pt = Camera.main.WorldToViewportPoint(transform.position);
				if (pt.x < 0 || pt.x > 1 || pt.y < 0 || pt.y > 1)
					Despawn();
				_frameSinceCheck = 0;
			}
		}
	}
}