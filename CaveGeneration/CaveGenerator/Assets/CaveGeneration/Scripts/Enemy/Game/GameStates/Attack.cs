using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.AI.FSM;
using System;

public class Attack : State {

    private Enemy enemy;
    private Vector2 directionalInput;

    private bool hasPlayerInSight = true;
    private void Awake() {
        enemy = GetComponent<Enemy>();
    }

    public override void StartState() {
        stateName = "Attack";
        hasPlayerInSight = true;

    }
    public override void Run() {

        Transform target = null;
        for (int i = 0; i < enemy.Eyes.visibleTargets.Count; i++) {
            Transform entity = enemy.Eyes.visibleTargets[i];
            if (entity.tag == "Player") {
                target = entity;
            }
        }

        if (target == null || transform == null) {
            enemy.FSM.StartState("Searching");
            return;
        }


        GetToPlayer(target);

       // enemy.Actor.SetDirectionalInput(directionalInput);
    }

    private void GetToPlayer(Transform target) {
        //Calculate what needs to be done to get to the target
        
        if(hasPlayerInSight) {
            enemy.Eyes.Focus();
            hasPlayerInSight = false;
        }

        //directionalInput = new Vector2(UnityEngine.Random.Range(.5f, 1), 0);
        var pos = enemy.Eyes.transform.position;
        var dir = target.position - pos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        enemy.Eyes.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    public override void Complete() {

    }
    public override void Transition(State fromState) {

    }
}
