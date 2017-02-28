using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.AI.FSM;
using System;

public class Attack : State {

    public override void StartState() {
        stateName = "Attack";
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
