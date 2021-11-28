using UnityEngine;
using UnityEngine.Events;

using Thuleanx.Input.Core;
using Thuleanx.Interaction;

namespace Thuleanx.Interaction.Core {
	[RequireComponent(typeof(PlayerDetector))]
	public class Interactor : MonoBehaviour {
		public PlayerDetector Detector {get; private set;}

		void Awake() {
			Detector = GetComponent<PlayerDetector>();
		}

		public PlayerInputProvider InputProvider;
		public UnityEvent OnInteract;


		void Update() {
			PlayerInputState Input = InputProvider.GetState() as PlayerInputState;
			if (Detector && Detector.Detected && Input.Interact && Input.CanInteract) {
				OnInteract?.Invoke();
				InputProvider.Feedback.InteractExecuted = true;
			}
		}
	}
}