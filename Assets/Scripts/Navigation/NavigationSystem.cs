using UnityEngine;
using UnityEngine.Events;

using System;

using Thuleanx.Input;
using Thuleanx.Input.Core;

namespace Thuleanx.Navigation {
	public class NavigationSystem : PlatformerMonoMiddleware {
		public static NavigationSystem Instance;

		public override void Awake() {
			base.Awake();
			Instance = this;
		}

		[HideInInspector]
		public bool Active = false;
		Vector2 Destination;
		Action OnEnd;

		public void Goto(Vector2 position) => Goto(position, CancelNavigation);
		public void Goto(Vector2 position, Action OnEnd) {
			if (!Active) {
				Active = true;
				Destination = position;
				this.OnEnd = OnEnd;
			} else Debug.Log("Please cancel previous navigation before starting a new one");
		}
		public void CancelNavigation() => Active = false;

		public override InputState Process(InputState state) {
			if (Active) {
				PlatformerInputState PIS = DefaultProvider.Value.BlankState() as PlatformerInputState; // ignore previous input states, completely overriding

				float dx = Destination.x - Context.ReferenceManager.Player.transform.position.x;
				if (Mathf.Abs(dx) < .1f) {
					Active = false;
					OnEnd?.Invoke();
					return state;
				}

				PIS.Movement.x = Mathf.Sign(dx);
				return PIS;
			}
			return state;
		}
		public override void Review(InputFeedback feedback) {

		}

		public override int GetPriority() => (int) MiddlewarePriority.NAVIGATION;
	}
}