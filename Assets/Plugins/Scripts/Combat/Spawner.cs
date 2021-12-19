using UnityEngine;

using Thuleanx.Optimization;
using Thuleanx.Utils;

namespace Thuleanx.Combat {
	public class Spawner : MonoBehaviour {
		public BubblePool Enemy;
		public float PositionalVariance;

		public void Spawn(int count) {
			while (count --> 0) 
				Enemy.Borrow(gameObject.scene, transform.position + Vector3.right * PositionalVariance * Calc.RandomRange(-1f, 1f));
		}
	}
}