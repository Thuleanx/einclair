using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

using Thuleanx.Navigation;
using Thuleanx.Input;
using Thuleanx.Input.Core;

namespace Thuleanx.Interaction.Core {
	public class NavigationBehaviour : PlayableBehaviour, InputMiddleware {
		public Vector2 Position;
		public PlayerInputProvider InputProvider;

		public int GetPriority() => (int)MiddlewarePriority.OVERRIDE;

		public void Attach(InputProvider provider) => provider?.AddMiddleware(this, GetPriority());
		public void Detach(InputProvider provider) => provider?.RemoveMiddleware(this, GetPriority());


		public override void OnBehaviourPlay(Playable playable, FrameData info) {
			base.OnBehaviourPlay(playable, info);

			if (info.evaluationType == FrameData.EvaluationType.Playback) {
				Attach(InputProvider);

				if (NavigationSystem.Instance != null) {
					playable.GetGraph().GetRootPlayable(0).SetSpeed(0);
					NavigationSystem.Instance.Goto(Position, () => {
						playable.GetGraph().GetRootPlayable(0).SetSpeed(1);
					});
				} else Debug.Log("No navigation system found. Proceed to simply lock input.");
			}
		}

		public override void OnBehaviourPause(Playable playable, FrameData info) {
			if (!Application.isPlaying) return;

			var duration = playable.GetDuration();
			var count = playable.GetTime() + info.deltaTime;

			if ((info.effectivePlayState == PlayState.Paused && count > duration) || playable.GetGraph().GetRootPlayable(0).IsDone()) {
				Detach(InputProvider);
			}
		}

		public InputState Process(InputState state) {
			PlayerInputState PIS = (PlayerInputState)state;
			PIS.Movement = Vector2.zero;
			PIS.Jump = false;
			PIS.JumpReleased = false;
			return PIS;
		}

		public void Review(InputFeedback feedback) { }
	}
}