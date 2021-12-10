using UnityEngine;
using UnityEngine.Events;

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

		[SerializeField] UnityEvent _OnEntrance;

		public enum DoorSide {
			From = 0,
			To = 1
		}
		public enum DoorState {
			Open = 1,
			Closed = 0
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

		public override void Awake() {
			Anim = GetComponent<Animator>();
			Collider = GetComponent<Collider2D>();
			CurrentState = DoorState.Closed;
			base.Awake();
		}

		public void Open() => CurrentState = DoorState.Open;
		public void Close() => CurrentState = DoorState.Closed;

		public override bool CanInteract() => _Current == DoorState.Closed;

		public void ProcessTransition() {
			Room previous = StoryMode.CurrentRoom;
			App.Instance.StartCoroutine(StoryMode.TransitionThrough(Passage));
		}

		public void ProcessEntrance() {
			Context.ReferenceManager.Player.transform.position = transform.position; // Teleport player to door
			_OnEntrance?.Invoke();
		}
	}
}