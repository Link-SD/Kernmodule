using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Actor : MonoBehaviour {

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;

    public float accelerationTimeAirbourne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    [Header("Jetpack")]
    public bool hasJetPack;

    public float jetPackFuel = 100;
    [Range(5, 20)]
    public float drainageRate = 10;

    //Wallsliding
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    private float timeToWallUnstick;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float gravity;

    private Vector3 velocity;

    private Controller2D controller;
    public Controller2D Controller {
        get { return controller; }
    }


    private float velocityXSmoothing;
    private Vector2 directionalInput;

    private bool wallSliding;
    private int wallDirX;

    private float btnHoldTimer = .3f;
    private float timer;

    public int FaceDir {
        get { return controller.collisions.faceDir; }
    }
    public Vector3 Velocity {
        get { return velocity; }
    }

    private void Awake() {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    private void Update() {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.IsGrounded()) {
            if (controller.collisions.slidingDownMaxSlope) {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            } else {
                velocity.y = 0;
            }

        }
        if (controller.IsGrounded() && hasJetPack) {
            if (jetPackFuel <= 100) {
                jetPackFuel += (drainageRate * 0.8f) * Time.deltaTime;
                timer = 0;
            }
        }

        HudManager.Instance.UpdateSliderValue(jetPackFuel);
    }

    public void SetDirectionalInput(Vector2 input) {
        directionalInput = input;
    }

    public void OnJumpInputDown() {
        if (wallSliding) {
            if (wallDirX == directionalInput.x) {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            } else if (directionalInput.x == 0) {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            } else {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.IsGrounded()) {
            /*   if (controller.collisions.slidingDownMaxSlope) {
                   if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) { // not jumping against max slope
                       velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                       velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                   }
               } else {
                   velocity.y = maxJumpVelocity;
               }*/
            velocity.y = maxJumpVelocity;
        }
    }

    public void OnJumpInputUp() {
        if (velocity.y > minJumpVelocity) {
            velocity.y = minJumpVelocity;
        }
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
    }

    public void OnJetPack() {

        if (jetPackFuel > 0) {
            if (timer >= btnHoldTimer) {
                gravity = 20;
                jetPackFuel -= drainageRate * Time.deltaTime;

            }
        }
        timer += Time.deltaTime;


    }

    private void HandleWallSliding() {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0) {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0) {
                    timeToWallUnstick -= Time.deltaTime;
                } else {
                    timeToWallUnstick = wallStickTime;
                }
            } else {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    private void CalculateVelocity() {
        float targetVelocityX = directionalInput.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirbourne);
        velocity.y += gravity * Time.deltaTime;
    }
}
