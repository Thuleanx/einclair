using UnityEngine;
using Thuleanx.Engine;

namespace Thuleanx.AI {
	public class Agent : MonoBehaviour {
		public PhysicsObject Body {get; private set; }
		public StateMachine StateMachine {get; protected set; }

		public virtual void Awake() {
			Body = GetComponent<PhysicsObject>();
			StateMachineSetup();
			ObjectSetup();
		}

		public virtual void StateMachineSetup() {}
		public virtual void ObjectSetup() {}

		void OnEnable() {
			StateMachine?.Init();
		}

		public virtual void Update() {
			StateMachine?.Update();
		}
		public virtual void FixedUpdate() {
			StateMachine?.FixedUpdate();
		}
	}
}