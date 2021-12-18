using UnityEngine;

using Thuleanx.Mapping;
using Thuleanx.Interaction.Core;
using Thuleanx.SceneManagement.Core;

namespace Thuleanx.Mapping.Core {
	public class Transitioner : MonoBehaviour {
		public StoryMode StoryMode;
		public Passage Passage;

		public void Transition() {
			App.Instance.StartCoroutine(StoryMode.TransitionThrough(Passage));
		}
	}
}