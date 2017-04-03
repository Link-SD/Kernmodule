using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SD.AI.Boids {

    public static class CallBacks {

        public static event Action OnEnemyDied = null;
        public static void IssueOnEnemyDied() {
            if (OnEnemyDied != null)
                OnEnemyDied.Invoke();
        }

        public static event Action OnSurvived = null;
        public static void IssueOnSurvived() {
            if (OnSurvived != null)
                OnSurvived.Invoke();
        }
    }
}