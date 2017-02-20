using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.AI.Boids {
    public class BoidsController : MonoBehaviour {

        public Boid boidPrefab;

        public int numberOfBoids;

        private List<Boid> flock = new List<Boid>();

        private void Start() {
            CreateFlock();
            
        }

        private void Update() {
            MoveFlock();
        }

        private void CreateFlock() {
            for (int i = 0; i < numberOfBoids; ++i) {
                CreateBoid();
            }
        }

        private void CreateBoid() {
            Boid go = Instantiate(boidPrefab, this.transform);
            Vector3 t = RandomPostition();
            go.transform.position = t;
            go.transform.rotation = new Quaternion(0, 0, UnityEngine.Random.rotation.z, UnityEngine.Random.rotation.w);

            flock.Add(go);
        }

        private Vector3 RandomPostition() {
            return Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), Camera.main.farClipPlane / 2));
        }

        private void MoveFlock() {
            Vector3 v1, v2, v3;

            foreach (Boid boid in flock) {
                v1 = Cohesion(boid);
                v2 = Seperation(boid);
                v3 = Alignment(boid);
                
                boid.Velocity = boid.Velocity + v1 + v2 + v3 * Time.deltaTime;
                boid.transform.position = boid.transform.position + boid.Velocity;
                boid.transform.up = boid.Velocity.normalized;

                
            }
        }

        private Vector2 Cohesion(Boid boid) {

            Vector3 pcJ = Vector3.zero;

            foreach (Boid b in flock) {
                if(b != boid) {
                    pcJ = pcJ + b.transform.position;
                }
            }
            
            pcJ = pcJ / (flock.Count - 1);
            
            return (pcJ - boid.transform.position) / 100;
        }

        private Vector2 Seperation(Boid boid) {

            Vector3 c = Vector3.zero;


            foreach (Boid b in flock) {

                if(b != boid) {
                      if ((b.transform.position - boid.transform.position).sqrMagnitude < 1 ) {
                         c = c - (b.transform.position - boid.transform.position);
                    }
                }
            }
            return c;
        }
        
        private Vector2 Alignment(Boid boid) {

            Vector3 pvJ = Vector3.zero;
            
            foreach (Boid b in flock) {
                if(b != boid) {
                    pvJ = pvJ + b.Velocity;
                }
            }

            pvJ = pvJ / (flock.Count - 1);

            return (pvJ - boid.Velocity / 8);
        }
    }
}
