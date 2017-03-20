using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SD.AI.Boids {
    public class LevelController : MonoBehaviour {
        public GameObject levelPrefab;

        public float speed = 2.0f;

        [Range(1,20)]
        public float gapSize = 10;
        [Range(1, 20)]
        public float timeTillNextSpawn = 2;
        public int LevelScore {
            get { return levelScore; }
        }

        private float xMin, xMax, yMin, yMax;
        private float objectHeight = 0;
        private int levelScore = 0;
        private List<GameObject> walls = new List<GameObject>();
        private float timer = 0;
        

        private void Start() {
            xMin = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.0f, 0)).x;
            xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1.0f, 0)).x;
            yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 0.0f)).y;
            yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height * 1.0f)).y;
            
       //     InvokeRepeating("SpawnWall", 0, spawnSpeed);
        }

        private void SpawnWall() {

            float s = UnityEngine.Random.Range(0, (xMax * 2) - gapSize);
            float r = (xMax * 2 - gapSize) - s;

            GameObject wall = new GameObject("Wall");
            wall.transform.SetParent(transform);

            GameObject leftWall;
            GameObject rightWall;

            leftWall = Instantiate(levelPrefab, wall.transform);
            leftWall.transform.localScale = new Vector2(s, 1);
            leftWall.transform.position = new Vector2(xMin + (leftWall.GetComponent<BoxCollider2D>().bounds.size.x / 2), 0);

            leftWall.name = "A";

            rightWall = Instantiate(levelPrefab, wall.transform);
            rightWall.transform.localScale = new Vector2(r, 1);
            rightWall.transform.position = new Vector2(xMax - (rightWall.GetComponent<BoxCollider2D>().bounds.size.x / 2), 0);

            rightWall.name = "B";


            objectHeight = leftWall.GetComponent<BoxCollider2D>().bounds.size.y;
            wall.transform.position = new Vector2(0, yMax + objectHeight);
            walls.Add(wall);
            levelScore++;
            gapSize -= 0.10f;
            CallBacks.IssueOnSurvived();
        }

        private void Update() {
            foreach (GameObject wall in walls) {
                wall.transform.Translate((Vector3.down * Time.deltaTime) * speed);
            }
            for (int i = 0; i < walls.Count; i++) {
                if (walls[i].transform.position.y < yMin - objectHeight) {
                    Destroy(walls[i].gameObject);
                    walls.Remove(walls[i]);
                }
            }

            if (CanSpawn()) {
                SpawnWall();
            }
        }

        private bool CanSpawn() {
            timer -= Time.deltaTime;

            if(timer <= 0f) {
                print("Time up");
                timer = timeTillNextSpawn;
                return true;
            }
            return false;
        }
    }
}