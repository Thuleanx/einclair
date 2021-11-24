using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace Thuleanx.AI {
	public class StateMachine {
		int currentState;
		int defaultState;
		public Action[] Begins, Ends;
		public Func<int>[] Updates, Transitions, FixUpdates;
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
			FixUpdates = new Func<int>[numbers];
			Transitions = new Func<int>[numbers];
			Coroutines = new Coroutine[numbers];
			this.defaultState = defaultState;
		}

		public void SetCallbacks(int state, Func<int> update, Func<int> fixUpdate, Func<int> transition, 
				Coroutine coroutine, Action begin, Action end) {

			SetCallbackTransition(state, transition);
			SetCallbackUpdate(state, update);
			SetCallbackFixUpdate(state, fixUpdate);
			SetCallbackCoroutine(state, coroutine);
			SetCallbackBegin(state, begin);
			SetCallbackEnd(state, end);
		}

		public void SetCallbackTransition(int state, Func<int> transition)  => Transitions[state] = transition;
		public void SetCallbackUpdate(int state, Func<int> update)  		=> Updates[state] = update;
		public void SetCallbackFixUpdate(int state, Func<int> update) 		=> FixUpdates[state] = update;
		public void SetCallbackCoroutine(int state, Coroutine coroutine)  	=> Coroutines[state] = coroutine;
		public void SetCallbackBegin(int state, Action begin)  				=> Begins[state] = begin;
		public void SetCallbackEnd(int state, Action end)  					=> Ends[state] = end;

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

		public void FixedUpdate() {
			if (FixUpdates[State] != null) {
				int nxt = FixUpdates[State].Invoke();
				if (nxt != -1) State = nxt;
			}
		}
	}
}