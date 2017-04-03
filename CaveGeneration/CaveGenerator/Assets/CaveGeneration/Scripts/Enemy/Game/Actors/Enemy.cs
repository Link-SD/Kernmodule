using SD.AI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Actor actor;
    public Actor Actor {
        get { return actor; }
    }
    private FiniteStateMachine fsm;
    public FiniteStateMachine FSM {
        get { return fsm; }
    }


    private FieldOfView eyes;
    public FieldOfView Eyes {
        get { return eyes; }
    }

    private Vector3 prevPosition;
    private Vector2 velocity;
    public Vector2 Velocity {
        get { return velocity; }
    }
    private void Start() {
        actor = GetComponent<Actor>();
        fsm = GetComponent<FiniteStateMachine>();
        eyes = transform.GetComponentInChildren<FieldOfView>();

        fsm.StartState("Idle");


    }

    private void Update() {

        velocity = (transform.position - prevPosition) / Time.deltaTime;

        if (Mathf.Abs(velocity.x) < .1f)
            velocity.x = 0;

        prevPosition = transform.position;

        if (actor.Controller.collisions.above && actor.Controller.collisions.collisionObject.tag == "Player") {
            Destroy(gameObject);
        }



        fsm.UpdateFSM();



    }
}
