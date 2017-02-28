using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SD.AI.FSM {
    public class FiniteStateMachine : MonoBehaviour {

        protected State currentState;
       
        private Dictionary<string, State> fsmStates;

        private State[] states;

        public void StartState(string stateName) {
            FetchStates();

            State state = null;

            if (fsmStates.TryGetValue(stateName, out state)) {
                SetState(state);
                state.StartState();
            } else {
                Debug.LogError("The state '" + stateName + "' could not be found.");
                return;
            }
        }

		public void UpdateFSM() {
			if (currentState != null) {
				currentState.Run();
			}
		}

		private void SetState(State newState) {
            State oldState = null;
			if (currentState != null) {
                oldState = currentState;
				currentState.onState -= SetState;
                currentState.Complete();
            }
            
			currentState = newState;
            if(oldState != null)
                currentState.Transition(oldState);

            currentState.onState += SetState;

		}

        private void FetchStates() {

            fsmStates = new Dictionary<string, State>();
            states = GetComponents<State>();

            if (states == null || states.Length == 0) {
                Debug.LogError("There are no states attached to this gameobject ('" + gameObject.name + "'). To use this system, please attach at least one state.");
                return;
            }

            foreach (State state in states) {
                fsmStates.Add(state.stateName, state);
            }
        }
	}
}