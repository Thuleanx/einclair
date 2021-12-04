using UnityEngine;
using System.Collections.Generic;

using Thuleanx.Input;

namespace Thuleanx.Input.Core {
	public class AttackerInputProvider : PlatformerInputProvider {
		public override InputState BlankState() => new AttackerInputState();
	}
}