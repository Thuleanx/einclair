using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Thuleanx.Optimization;

namespace Thuleanx.Utils {
	public class General {
		public static IEnumerator _InvokeNextFrame(Action action) {
			yield return null;
			action?.Invoke();
		}

		public static Vector2 Rotate(Vector2 root, float rad) {
			return new Vector2( 
				root.x * Mathf.Cos(rad) - root.y * Mathf.Sin(rad),
        		root.x * Mathf.Sin(rad) + root.y * Mathf.Cos(rad)
			);
		}

		public static bool OnScreen(Vector2 pos) {
			Vector2 vp = Camera.main.WorldToViewportPoint(pos);
			return vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1;
		}

		public static void TryDispose(GameObject obj) {
			Bubble bubble = obj.GetComponent<Bubble>();
			if (bubble) bubble.Dispose();
			else obj.SetActive(false);
		}
	}
}