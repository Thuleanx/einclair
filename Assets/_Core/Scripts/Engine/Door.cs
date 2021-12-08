using UnityEngine;

using Thuleanx.Mapping;
using Thuleanx.Interaction.Core;
using Thuleanx.SceneManagement.Core;

namespace Thuleanx.Engine.Core {
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Collider2D))]
	public class Door : Interactor {
		public Animator Anim {get; private set; }
		public Collider2D Collider {get; private set; }

		public StoryMode StoryMode;
		public Passage Passage;
		public DoorSide side;

		public enum DoorSide {
			From = 0,
			To = 1
		}
		public enum DoorState {
			Open = 0,
			Closed = 1
		}
		DoorState _Current;
		public DoorState CurrentState {
			get => _Current;
			set {
				_Current = value;
				Anim?.SetInteger("State", (int) _Current);
				Collider.enabled = _Current == DoorState.Closed;
			}
		 }

		void Awake() {
			Anim = GetComponent<Animator>();
			Collider = GetComponent<Collider2D>();

			CurrentState = DoorState.Closed;
		}

		public override bool CanInteract() {
			return _Current == DoorState.Closed;
		}

		public void ProcessTransition() {
			Room previous = StoryMode.CurrentRoom;
			StoryMode.TransitionThrough(Passage);
		}
	}
}