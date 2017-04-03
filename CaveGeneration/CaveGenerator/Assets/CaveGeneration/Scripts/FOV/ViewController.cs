using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour {

    public float moveSpeed = 6;

    Camera viewCamera;
    Vector3 velocity;

    void Start() {
        viewCamera = Camera.main;
    }

    void Update() {

        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate() {
    }
}