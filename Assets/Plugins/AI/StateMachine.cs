using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace Thuleanx.AI {
	public class StateMachine {
		int currentState;
		int defaultState;
		public Action[] Begins, Ends;
		public Func<int>[] Updates, Transitions;
		public Coroutine[] Coroutines;
		
		public int State {
			get { return currentState; }
			set {
				if (value != currentState) {
					Ends[currentState]?.Invoke();
					currentState = value;
					Begins[currentState]?.Invoke();
				}
			}
		}

		public StateMachine(int numbers, int defaultState) {
			Begins = new Action[numbers];
			Ends = new Action[numbers];
			Updates = new Func<int>[numbers];
			Transitions = new Func<int>[numbers];
			Coroutines = new Coroutine[numbers];
			this.defaultState = defaultState;
		}

		public void SetCallbacks(int state, Func<int> update, Func<int> transition, Coroutine coroutine, Action begin, Action end) {
			Coroutines[state] = coroutine;
			Updates[state] = update;
			Transitions[state] = transition;
			Begins[state] = begin;
			Ends[state] = end;
		}

		public void Init() {
			currentState = defaultState;
			Begins[currentState]?.Invoke();
		}

		public void Update() {
			while (Transitions[State] != null) {
				int nxt = Transitions[State].Invoke();
				if (nxt == State || nxt == -1) break;
				State = nxt;
			}
			if (Updates[State] != null) {
				int nxt = Updates[State].Invoke();
				if (nxt != -1) State = nxt;
			}
		}
	}
}