using UnityEngine;
using System.Collections.Generic;
using System;

using Thuleanx.AI;
using Thuleanx.Utils;

namespace Thuleanx.Engine {
	[RequireComponent(typeof(PlatformEffector2D))]
	[RequireComponent(typeof(Collider2D))]
	public class MovingPlatform : Agent {
		[Serializable]
		public enum LoopMode {
			PINGPONG,
			LOOP
		}
		public enum MovingPlatformState {
			MOVING,
			SUSTAIN
		}


		[SerializeField] List<Checkpoint> checkpoints = new List<Checkpoint>();
		[SerializeField] LoopMode loopingMode = LoopMode.PINGPONG;
		[SerializeField] float timeBetweenCheckpoint = 5f;

		#region 

		int _checkpointID;
		int _NextCheckpoint {
			get {
				int n = checkpoints.Count;
				if (loopingMode == LoopMode.PINGPONG) {
					int x = _checkpointID % (2*(n - 1));
					return (n-1) - Mathf.Abs(n - 1 - x);
				} else if (loopingMode == LoopMode.LOOP) {
					return _checkpointID % n;
				}
				return 0;
			}
		}

		#endregion

		public override void StateMachineSetup() {
			StateMachine = new StateMachine(Enum.GetNames(typeof(MovingPlatformState)).Length, (int) MovingPlatformState.MOVING);

			StateMachine.SetCallbacks((int) MovingPlatformState.MOVING, 
				MovementUpdate, null, MovementTransition, null, MovementEnter, null);
			StateMachine.SetCallbacks((int) MovingPlatformState.SUSTAIN, 
				null, null, SustainTransition, null, SustainEnter, SustainExit);
			StateMachine.Init();
		}

		public override void Update() {
			base.Update();
		}


		void OnEnable() {
			_checkpointID = 1;
			transform.position = checkpoints[0].position;
		}

		#region Movement

		float _Movement_startTime;
		Vector2 _Movement_StartPos;
		void MovementEnter() {
			_Movement_startTime = Time.time;
			_Movement_StartPos = transform.position;
		}
		int MovementTransition() => (int) (Time.time - _Movement_startTime >= timeBetweenCheckpoint ? 
			MovingPlatformState.SUSTAIN :MovingPlatformState.MOVING);
		protected virtual int MovementUpdate() {
			float p = Easing.EaseInOutQuad((Time.time - _Movement_startTime), timeBetweenCheckpoint);
			transform.position = Vector2.Lerp(_Movement_StartPos, checkpoints[_NextCheckpoint].transform.position, p);
			return -1;
		}
		#endregion

		#region Sustain
		Timer _Sustain_sustaining;


		void SustainEnter() {
			_Sustain_sustaining = new Timer(checkpoints[_NextCheckpoint].duration);
			_checkpointID++;
			_Sustain_sustaining.Start();
		}
		int SustainTransition() {
			if (!_Sustain_sustaining) return (int) MovingPlatformState.MOVING;
			return (int) MovingPlatformState.SUSTAIN;
		}
		void SustainExit() {
		}
		#endregion

		void OnCollisionEnter2D(Collision2D other) {

		}

		void OnCollisionExit2D(Collision2D other) {
			
		}

		void OnDrawGizmos() {
			foreach (Checkpoint point in checkpoints)
				Gizmos.DrawWireSphere(point.position, .5f);
			for (int i = 0; i < checkpoints.Count-1; i++) {
				Gizmos.DrawLine(checkpoints[i].position, checkpoints[i+1].position);
			}
		}
	}
}