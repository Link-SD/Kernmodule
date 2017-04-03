using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.AI.Boids {
    public class BoidsController : MonoBehaviour {

        public Boid boidPrefab;

        public int numberOfBoids;

        public Transform enemy;

        public int BoidsAlive {
            get { return flock.Count; }
        }

        private List<Boid> flock = new List<Boid>();

        private float maxVelocity = .1f;

        private float xMin, xMax, yMin, yMax;
        private Vector3 v1, v2, v3, v4, v5;
        private float buffer = 0;

        private bool useMouse = true;

        private Vector2 mousePos;

        [SerializeField]
        public float cohesionFactor = 1, seperationFactor = 1, alignmentFactor = 1;

        private void Start() {
            CreateFlock();
            xMin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.0f, 0)).x;
            xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1.0f, 0)).x;
            yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.0f)).y;
            yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 1.0f)).y;
        }

        private void Update() {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (Input.GetMouseButtonDown(0))
                useMouse = !useMouse;

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

            for (int i = 0; i < flock.Count; i++) {
                if (flock.Count == 1) {
                    //Just one left;
                
                    v4 = BoundPosition(flock[i]);
                    v5 = GoToMousePosition(flock[i]);
                    flock[i].Velocity += (v4 + v5) * Time.deltaTime;

                } else {
                    v1 = Cohesion(flock[i]) * cohesionFactor;
                    v2 = Seperation(flock[i]) * seperationFactor;
                    v3 = Alignment(flock[i]) * alignmentFactor;
                    v4 = BoundPosition(flock[i]);
                    v5 = GoToMousePosition(flock[i]);

                    flock[i].Velocity += (v1 + v2 + v3 + v4 + v5) * Time.deltaTime;
                }
               
                LimitVelocity(flock[i]);
                flock[i].transform.position += flock[i].Velocity;

                flock[i].transform.up = flock[i].Velocity.normalized;

                BoundaryCheck(flock[i]);
            }
            /*
            foreach (Boid boid in flock) {
                v1 = Cohesion(boid) * cohesionFactor;
                v2 = Seperation(boid) * seperationFactor;
                v3 = Alignment(boid) * alignmentFactor;
                v4 = BoundPosition(boid);
                v5 = GoToMousePosition(boid);
                boid.Velocity += (v1 + v2 + v3 + v4 + v5) * Time.deltaTime;
                LimitVelocity(boid);
                boid.transform.position += boid.Velocity;

                boid.transform.up = boid.Velocity.normalized;
            }*/
        }

      

        private Vector2 Cohesion(Boid boid) {

            
            Vector3 pcJ = Vector3.zero;

            /*foreach (Boid b in flock) {
                if (b != boid) {
                    pcJ = pcJ + b.transform.position;
                }
            }*/
            for (int i = 0; i < flock.Count; i++) {
                if (flock[i] != boid) {
                    pcJ = pcJ + flock[i].transform.position;
                }
            }
            
            pcJ = pcJ / (flock.Count - 1);
            
            return (pcJ - boid.transform.position) / 100;
        }

        private Vector2 Seperation(Boid boid) {

            Vector3 c = Vector3.zero;

            for (int i = 0; i < flock.Count; i++) {
                if (flock[i] != boid) {
                    if ((flock[i].transform.position - boid.transform.position).sqrMagnitude < 1) {
                        c = c - (flock[i].transform.position - boid.transform.position);
                    }
                }
            }
/*
            foreach (Boid b in flock) {
                if(b != boid) {
                    
                    if ((b.transform.position - boid.transform.position).sqrMagnitude < 1 ) {
                         c = c - (b.transform.position - boid.transform.position);
                    }
                }
            }*/
            return c;
        }
        
        private Vector2 Alignment(Boid boid) {

            if (useMouse)
                return Vector2.zero;

            Vector3 pvJ = Vector3.zero;
            /*
            foreach (Boid b in flock) {
                if(b != boid) {
                    pvJ += b.Velocity;
                }
            }*/
            for (int i = 0; i < flock.Count; i++) {
                if (flock[i] != boid) {
                    pvJ += flock[i].Velocity;
                }
            }

            pvJ = pvJ / (flock.Count - 1);
            
            return (pvJ - boid.Velocity) / 1f;
        }

        private void LimitVelocity(Boid boid) {
            if(boid.Velocity.magnitude > maxVelocity) {
                boid.Velocity = (boid.Velocity / boid.Velocity.magnitude) * maxVelocity;
            }
        }

        private Vector3 BoundPosition(Boid boid) {

            Vector3 v = Vector3.zero;

            if (boid.transform.position.x < xMin - buffer) {
                  v.x = xMax;
            } else if (boid.transform.position.x > xMax + buffer) {
                v.x = xMin;
            }

            if (boid.transform.position.y < yMin - buffer) {
                v.y = yMax;
            } else if (boid.transform.position.y > yMax + buffer) {
                v.y = yMin;
            } 
            return v;
        }

        private void BoundaryCheck(Boid boid) {
            if (boid.transform.position.y < yMin - buffer) {
                flock.Remove(boid);
                Destroy(boid.gameObject);
                CallBacks.IssueOnEnemyDied();
            }
        }

        private Vector3 GoToMousePosition(Boid b) {

            if (!useMouse)
                return Vector2.zero;

            Vector3 place = mousePos;

            return (place - b.transform.position) / 100;
        }
    }
}
