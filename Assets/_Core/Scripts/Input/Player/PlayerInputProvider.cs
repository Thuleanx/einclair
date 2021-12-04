using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	[CreateAssetMenu(fileName = "PIP", menuName = "~Einclair/PlayerInputProvider", order = 0)]
	public class PlayerInputProvider : PlatformerInputProvider {
		public override InputState BlankState() => new PlayerInputState();
	}
}