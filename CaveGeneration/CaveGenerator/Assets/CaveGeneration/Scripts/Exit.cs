using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {
    
    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Player") {
            print("Level Completed");
        }

    }
}
