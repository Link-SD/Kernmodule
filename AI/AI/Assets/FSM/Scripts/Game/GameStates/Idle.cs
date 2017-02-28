using UnityEngine;
using System.Collections;
using SD.AI.FSM;
using System;

public class Idle : State {

    public override void StartState() {
        stateName = "Idle";
        base.StartState();
    }
    public override void Run() {
        base.Run();
	}
    public override void Complete() {
        base.Complete();
    }
    public override void Transition(State fromState) {
        base.Transition(fromState);
    }
}
