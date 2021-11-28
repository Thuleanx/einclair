using UnityEngine;
using UnityEngine.Timeline;

using Thuleanx.Interaction.Core;

namespace Thuleanx.Sequencer.Core {
	[TrackBindingType(typeof(GameObject))]
	[TrackClipType(typeof(NavigationClip))]
	public class InteractionSequencer : TrackAsset {
	}
}
