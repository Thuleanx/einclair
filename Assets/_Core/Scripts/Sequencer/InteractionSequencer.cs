using UnityEngine;
using UnityEngine.Timeline;

using Thuleanx.Interaction;

namespace Thuleanx.Sequencer.Core {
	[TrackBindingType(typeof(GameObject))]
	[TrackClipType(typeof(NavigationClip))]
	public class InteractionSequencer : TrackAsset {
	}
}
