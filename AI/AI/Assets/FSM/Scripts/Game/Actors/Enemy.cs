using SD.AI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private FiniteStateMachine fsm;

    private void Start() {
        fsm = GetComponent<FiniteStateMachine>();

        fsm.StartState("Idle");
    }

    private void Update() {
        fsm.UpdateFSM();

        if (Input.GetKeyDown(KeyCode.Space)) {
            fsm.StartState("Attack");
        }
    }
}
