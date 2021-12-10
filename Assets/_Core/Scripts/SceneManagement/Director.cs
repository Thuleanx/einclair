using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Thuleanx.Optimization;

using Thuleanx.Utils;

namespace Thuleanx.SceneManagement.Core {
	public class Director : MonoBehaviour {
		public static Director Instance;

		public enum Mode {
			Story,
			MainMenu
		}

		bool _isSwitching = false;
		[HideInInspector] GameMode _current_mode;
		public List<GameMode> GameModes = new List<GameMode>();

		void Awake() {
			Instance = this;
		}

		void Start() {
			Boot();
		}

		public void Boot() {
			switch (SceneManager.GetActiveScene().buildIndex) {
				case 0: 
					HandleStartRequested(GameModes[0]);
					break;
				case 1:
					_current_mode = GameModes[0];
					StartCoroutine(_current_mode.OnStart());
					break;
				default:
					_current_mode = GameModes[1];
					StartCoroutine(_current_mode.OnStart());
					break;
			}
		}

		private IEnumerator SwitchMode(GameMode mode) {
			yield return new WaitUntil(() => !_isSwitching);
			if (_current_mode == mode) yield break;

			_isSwitching = true;

			if (_current_mode != null) yield return _current_mode.OnEnd();
			_current_mode = mode;
			yield return _current_mode.OnStart();

			_isSwitching = false;
		}

		void HandleStartRequested(GameMode mode) {
		}
	}
}