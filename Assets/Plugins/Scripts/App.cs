using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;	

namespace Thuleanx {
	public class App : MonoBehaviour {
		public static float Gravity => Mathf.Abs(Physics2D.gravity.y);

		public static App Instance;

		void Awake() { }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Bootstrap() {
			if (FindObjectOfType<App>() == null) {
				var app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
				if (app == null) throw new ApplicationException();
				Instance = app.GetComponent<App>();
				UnityEngine.Object.DontDestroyOnLoad(app);
			}
		}

	}
}