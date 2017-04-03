using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Actor))]
public class PlayerInput : MonoBehaviour {

    private Actor actor;

	// Use this for initialization
	void Start () {
        actor = GetComponent<Actor>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        actor.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.Space)) {
            actor.OnJumpInputDown();

        }
        if (Input.GetKey(KeyCode.Space)) {
                //use jetpack
                actor.OnJetPack();
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            actor.OnJumpInputUp();
        }
    }
}
