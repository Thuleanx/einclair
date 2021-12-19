using UnityEngine;

namespace Thuleanx.SceneManagement.Core {
	public class ToStory : MonoBehaviour {
		public void Embark() {
			Director.Instance.HandleSwitch(1);
		}
	}
}