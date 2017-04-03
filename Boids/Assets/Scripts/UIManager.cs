using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SD.AI.Boids {
    public class UIManager : MonoBehaviour {

        public Text scoreText;
        public Text boidsText;

        private BoidsController boidsController;
        private LevelController levelController;
        // Use this for initialization
        void Start() {
            boidsController = FindObjectOfType<BoidsController>();
            levelController = FindObjectOfType<LevelController>();
            scoreText.text = "Score: " + levelController.LevelScore.ToString();
            boidsText.text = "Boids Alive: " + boidsController.numberOfBoids.ToString();
            CallBacks.OnEnemyDied += UpdateUI;
            CallBacks.OnSurvived += UpdateUI;
        }

        private void UpdateUI() {
            scoreText.text = "Score: " + levelController.LevelScore.ToString();
            boidsText.text = "Boids Alive: " + boidsController.BoidsAlive.ToString();
        }
    }
}