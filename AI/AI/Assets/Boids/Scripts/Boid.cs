using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.AI.Boids {
    public class Boid : MonoBehaviour {

        private Vector3 velocity;

        public Vector3 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
    }
}
