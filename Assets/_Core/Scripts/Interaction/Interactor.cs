using UnityEngine;
using UnityEngine.Events;

using Thuleanx.Input.Core;
using Thuleanx.Manager.Core;

namespace Thuleanx.Interaction.Core {
	[RequireComponent(typeof(PlayerDetector))]
	public class Interactor : MonoBehaviour {
		public PlayerDetector Detector {get; private set;}

		void Awake() {
			Detector = GetComponent<PlayerDetector>();
		}

		public UnityEvent OnInteract;

		void Update() {
			PlayerInputState Input = GlobalReferences.PlayerInputProvider.GetState() as PlayerInputState;
			if (Detector && Detector.Detected && Input.Interact && Input.CanInteract) {
				OnInteract?.Invoke();
				GlobalReferences.PlayerInputProvider.Feedback.InteractExecuted = true;
			}
		}
	}
}