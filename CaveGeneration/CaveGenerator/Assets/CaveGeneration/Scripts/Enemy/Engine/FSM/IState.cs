using UnityEngine;
using System.Collections;

namespace SD.AI.FSM {
	public delegate void StateEvent(State newState);
	public interface IState {
		event StateEvent onState;
        string GetStateName { get; }

        void StartState();
        void Transition(State fromState);
		void Run();
        void Complete();
	}
}