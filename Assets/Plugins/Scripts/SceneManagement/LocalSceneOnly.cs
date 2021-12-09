using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thuleanx.SceneManagement {
	public class LocalSceneOnly : MonoBehaviour {
		void Awake() {
			SceneManager.activeSceneChanged += ChangeActiveScene;
			ChangeActiveScene(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
		}

		void ChangeActiveScene(Scene current, Scene next) {
			foreach (Transform child in transform) {
				child.gameObject.SetActive(next == gameObject.scene);
			}
		}

		void OnDisable() {
			SceneManager.activeSceneChanged -= ChangeActiveScene;
		}
	}
}