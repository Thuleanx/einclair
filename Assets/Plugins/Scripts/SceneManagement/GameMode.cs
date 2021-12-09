using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Thuleanx.SceneManagement {
	public class GameMode : ScriptableObject {
		public enum State {
			Loading, 
			Starting, 
			Started,
			Ending,
			Ended
		}
		[HideInInspector]
		public State _state = State.Loading;
		
		public virtual IEnumerator OnStart() { yield return null; }
		public virtual IEnumerator OnEnd() { yield return null; }

		public virtual void Pause() => Time.timeScale = 0f;
		public virtual void Resume() => Time.timeScale = 1f;
	}
}