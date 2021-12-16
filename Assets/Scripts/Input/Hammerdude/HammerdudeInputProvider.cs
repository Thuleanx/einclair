using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	[CreateAssetMenu(fileName = "HammerdudeInputProvider", menuName = "~Einclair/HammerdudeInputProvider", order = 0)]
	public class HammerdudeInputProvider : PlatformerInputProvider {
		public override InputState BlankState() => new HammerdudeInputState();
	}
}