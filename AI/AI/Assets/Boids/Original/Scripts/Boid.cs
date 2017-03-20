using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.AI.Boids {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Boid : MonoBehaviour {

        private Rigidbody2D rb;

        public Vector3 Velocity {
            get { return rb.velocity; }
            set { if(rb != null) rb.velocity = value; }
        }

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }
    }
}
