using UnityEngine;
using System.Collections;
using SD.AI.FSM;
using System;

public class Searching : State {
    private Enemy enemy;

    private void Awake() {
        enemy = GetComponent<Enemy>();
    }
    public override void StartState() {
        stateName = "Searching";
        enemy.FSM.StartState("Idle");
        //    base.StartState();
    }
    public override void Run() {
  //      base.Run();
	}
    public override void Complete() {
  //      base.Complete();
    }
    public override void Transition(State fromState) {
  //      base.Transition(fromState);
    }
}
