using UnityEngine;
using UnityEngine.SceneManagement;

using Thuleanx.Utils;

namespace Thuleanx.SceneManagement.Core {
	public class ToEnd : MonoBehaviour {
		public SceneReference EndingScene;
		public void Embark() {
			App.Instance.RequestLoad(EndingScene.SceneName);
			Director.Instance.HandleSwitch(0);
		}
	}
}