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
				foreach (Passage passage in CurrentRoom.AdjacentPassages)
					if (passage.LoadBoth && passage.GetOther(CurrentRoom).Scene.Validated())
						passage.GetOther(CurrentRoom).Scene.LoadSceneAsync(LoadSceneMode.Additive);
				_state = GameMode.State.Started;
			} else {
				if (_state != GameMode.State.Ended && _state != GameMode.State.Loading)
					yield break;
				_state = GameMode.State.Starting;

				// activeRooms = new List<Room>();
				CurrentRoom = Map.RootNode;
				yield return CurrentRoom.Scene.LoadSceneAsync(LoadSceneMode.Single);
				// Load adjacent scenes
				foreach (Passage passage in CurrentRoom.AdjacentPassages)
					if (passage.LoadBoth && passage.GetOther(CurrentRoom).Scene.Validated())
						passage.GetOther(CurrentRoom).Scene.LoadSceneAsync(LoadSceneMode.Additive);
				// ReloadCurrentRoom();
				// activeRooms.Add(CurrentRoom);

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


				// List<Room> nextActiveRooms = new List<Room>();
				// HashSet<string> allowedRooms = new HashSet<string>();

				// if (!SceneManager.GetSceneByName(room.Scene.SceneName).isLoaded) {
				// 	room.Scene.LoadScene(LoadSceneMode.Single);
				// 	activeRooms.Add(room);
				// }
				// allowedRooms.Add(room.Scene.SceneName);
				// foreach (Passage passage in room.AdjacentPassages)
				// 	if (passage.LoadBoth)
				// 		allowedRooms.Add(passage.GetOther(room).Scene.SceneName);
				
				// foreach (Room loadedRoom in activeRooms) {
				// 	if (!allowedRooms.Contains(loadedRoom.Scene.SceneName))
				// 		loadedRoom.Scene.UnloadScene();
				// 	else nextActiveRooms.Add(loadedRoom);
				// }
				// foreach (Passage passage in room.AdjacentPassages) {
				// 	if (passage.LoadBoth) {
				// 		Room other = passage.GetOther(room);
				// 		other.Scene.LoadSceneAsync(LoadSceneMode.Additive);
				// 		nextActiveRooms.Add(other);
				// 	}
				// }

				// CurrentRoom = room;
				// activeRooms = nextActiveRooms;
				CurrentRoom = room;
				yield return CurrentRoom.Scene.LoadSceneAsync(LoadSceneMode.Single);
				foreach (Passage passage in CurrentRoom.AdjacentPassages)
					if (passage.LoadBoth && passage.GetOther(CurrentRoom).Scene.Validated())
						passage.GetOther(CurrentRoom).Scene.LoadSceneAsync(LoadSceneMode.Additive);

				// Find Player and places him in the correct place.
				foreach (Door door in FindObjectsOfType<Door>())
					if (door.Passage == passageTo)
						door.ProcessEntrance();
			}
			yield return null;
		}

		// public void LoadExtraRoom(Room room) {
		// 	if (!SceneManager.GetSceneByName(room.Scene.SceneName).isLoaded) {
		// 		room.Scene.LoadSceneAsync();
		// 		activeRooms.Add(room);
		// 	}
		// }

		// public void UnloadExtraRoom(Room room) {
		// 	if (room != CurrentRoom && SceneManager.GetSceneByName(room.Scene.SceneName).isLoaded) {
		// 		room.Scene.UnloadScene();
		// 		activeRooms.Remove(room);
		// 	}
		// }
	}
}