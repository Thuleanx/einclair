using UnityEngine;
using System.Collections.Generic;

namespace Thuleanx.Audio {
	public class SoundBoard : MonoBehaviour {
		[FMODUnity.EventRef] public List<string> Event = new List<string>();

		public void PlaySound(int id) {
			AudioManager.Instance.PlayOneShot(Event[id]);
		}

		public void PlaySound3D(int id) {
			AudioManager.Instance.PlayOneShot3D(Event[id], transform.position);
		}
	}
}