using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Thuleanx.Interaction {
	public abstract class CustomEmitter : SignalEmitter {
		public abstract void Execute();
	}
}