using UnityEngine;

using Thuleanx.Input;

namespace Thuleanx.AI.Core {
	public class PlatformerAI : Agent {
		#region Components
		public Animator Anim {get; private set; }

		public InputProvider Provider;
		#endregion

		public override void ObjectSetup() {
			Anim = GetComponent<Animator>();
		}
	}
}