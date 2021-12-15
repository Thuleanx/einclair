using UnityEngine;
using System.Collections.Generic;

namespace Thuleanx.Input.Core {
	public class LoadPlayerInputProvider : MonoBehaviour {
		public PlayerInputProvider InputProvider;
		public List<MonoMiddleware> Middlewares = new List<MonoMiddleware>();

		void Awake() {
			foreach (var middle in Middlewares) 
				middle.Attach(InputProvider);
		}

		void OnDestroy() {
			foreach (var middle in Middlewares) 
				middle.Detach(InputProvider);
		}

		void LateUpdate() {
			InputProvider.ProcessFeedback();
		}
	}
}
