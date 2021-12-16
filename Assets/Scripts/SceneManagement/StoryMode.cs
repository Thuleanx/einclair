using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using Thuleanx.Utils;
using Thuleanx.Mapping;
using Thuleanx.Engine.Core;

namespace Thuleanx.SceneManagement.Core {
	[CreateAssetMenu(fileName = "StoryMode", menuName = "~Einclair/StoryMode", order = 0)]
	public class StoryMode : GameMode {
		public MapGraph Map;
		// [HideInInspector] public List<Room> activeRooms;

		public Room CurrentRoom {  get; private set;  }

		public override IEnumerator OnStart() {
			Pause();
			if (App.Instance.IsEditor) {
				_state = GameMode.State.Starting;
				string _activeSceneName = SceneManager.GetActiveScene().name;
				foreach (var node in Map.nodes) 
					if (node is Room && (node as Room).Scene.SceneName == _activeSceneName)
						CurrentRoom = node as Room;
				LoadAdjacents(CurrentRoom);
				_state = GameMode.State.Started;
			} else {
				if (_state != GameMode.State.Ended && _state != GameMode.State.Loading)
					yield break;
				_state = GameMode.State.Starting;

				CurrentRoom = Map.RootNode;
				yield return App.Instance.DirectLoadAsync(CurrentRoom.Scene.SceneName, LoadSceneMode.Single);
				// Load adjacent scenes
				LoadAdjacents(CurrentRoom);

				_state = GameMode.State.Started;
			}
			Resume();
			yield return null;
		}
		public override IEnumerator OnEnd() {
			if (_state != GameMode.State.Started) yield break;
			Pause();
			_state = GameMode.State.Ending;
			_state = GameMode.State.Ended;
			Resume();
			yield return null;
		}
		public IEnumerator TransitionThrough(Passage passageTo) {
			if (passageTo.From == CurrentRoom || passageTo.To == CurrentRoom) {
				Room room = passageTo.GetOther(CurrentRoom);

				// CurrentRoom = room;
				// activeRooms = nextActiveRooms;
				CurrentRoom = room;
				yield return CurrentRoom.Scene.LoadSceneAsync(LoadSceneMode.Single);
				LoadAdjacents(CurrentRoom);

				// Find Player and places him in the correct place.
				foreach (Door door in FindObjectsOfType<Door>())
					if (door.Passage == passageTo)
						door.ProcessEntrance();
			}
			yield return null;
		}

		public void LoadAdjacents(Room room) {
			if (room != null)
				foreach (Passage passage in room.AdjacentPassages)
					if (passage.LoadBoth && passage.GetOther(room).Scene.Validated())
						App.Instance.RequestLoadAsync(passage.GetOther(CurrentRoom).Scene.SceneName, LoadSceneMode.Additive);
		}
	}
}