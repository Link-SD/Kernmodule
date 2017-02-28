using UnityEngine;
using System.Collections;
using System;

namespace SD.AI.FSM {
    public abstract class State : MonoBehaviour, IState {

        public string stateName = "Default";
        public string GetStateName {
            get {
                return stateName;
            }
        }

        public event StateEvent onState;

        public virtual void StartState() {
            Debug.Log("Started " + this);
        }

		public virtual void Run() {
            Debug.Log("Running " + this);
        }

        public virtual void Transition(State fromState) {
            Debug.Log("from " + fromState + " to " + this);
        }

        public virtual void Complete() {
            Debug.Log("Completed " + this);
        }
    }
}