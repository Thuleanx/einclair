using UnityEngine;

using Thuleanx.AI.Core;

namespace Thuleanx.Manager.Core {
	public class ReferenceManager : MonoBehaviour {
		[SerializeField] Player _player;

		public Player Player {get => _player ? _player : FindObjectOfType<Player>(); }
	}
}