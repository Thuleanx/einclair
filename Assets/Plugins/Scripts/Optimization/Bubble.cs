using UnityEngine;
using Thuleanx.Utils;

namespace Thuleanx.Optimization {
	public class Bubble : MonoBehaviour {
		[HideInInspector] public BubblePool Pool;
		[HideInInspector] public bool InPool = false;

		void OnDisable() { 
			if (this != null && BubbleManager.Instance != null)
				BubbleManager.Instance?.StartCollect(this); 
		}
	}
}