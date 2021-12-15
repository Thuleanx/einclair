using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Thuleanx.Utils;

namespace Thuleanx.Optimization {
	public class BubbleManager : MonoBehaviour {
		public static BubbleManager Instance;
		public List<BubblePool> ActivePools;

		public float ShrinkTimerSeconds = 30;
		Timer _ShrinkTimer;

		void Awake() {
			Instance = this;
		}

		void Update() {
			if (!_ShrinkTimer) {
				foreach (var pool in ActivePools)
					pool.Shrink();
				_ShrinkTimer = new Timer(ShrinkTimerSeconds);
				_ShrinkTimer.Start();
			}
		}
	}
}