using UnityEngine;
using System.Collections;
using System;

public class Controller2D : RaycastController {

    public float maxSlopeAngle = 80;

    public CollisionInfo collisions;

    private Vector2 input;
    private Vector2 moveAmount;

    public Vector2 Input {
        get { return input; }
    }

    protected override void Awake() {
        base.Awake();
        collisions.faceDir = 1;

    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform) {
        Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false) {
        this.moveAmount = moveAmount;
        UpdateRaycastOrigins();

        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        this.input = input;

        if (moveAmount.y < 0) {
            DescendSlope(ref moveAmount);
        }

        if (moveAmount.x != 0) {
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount);
        if (moveAmount.y != 0) {
            VerticalCollisions(ref moveAmount);
        }
        Headbump();
        transform.Translate(moveAmount);

        

        if (standingOnPlatform) {
            collisions.below = true;
        }
    }

    private void HorizontalCollisions(ref Vector2 moveAmount) {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;

        if (Mathf.Abs(moveAmount.x) < SKIN_WIDTH) {
            rayLength = 2 * SKIN_WIDTH;
        }

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit) {

                if (hit.distance == 0) {
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxSlopeAngle) {
                    if (collisions.descendingSlope) {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - SKIN_WIDTH;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle) {
                    moveAmount.x = (hit.distance - SKIN_WIDTH) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope) {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                    collisions.collisionObject = hit.collider.gameObject;
                }
            }
        }
    }

    public bool IsGrounded() {
        return collisions.below;
    }

    public bool IsTouchingCeiling() {
        return collisions.above;
    }

    private void Headbump() {
        float rayLength = SKIN_WIDTH;
        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up, Color.blue);

            if (hit) {
                collisions.above = true;
                collisions.collisionObject = hit.collider.gameObject;
                rayLength = hit.distance;
            }
        }
    }

    public bool CanWalk() {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
        //Het spijt me voor deze code. PLS GOOI ME NIET VAN DE OPLEIDING
        Debug.DrawRay(rayOrigin, (directionX == 1)? (Vector2.right + Vector2.down) : (Vector2.left + Vector2.down) * -directionX, Color.yellow);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, (directionX == 1) ? (Vector2.right + Vector2.down) : (Vector2.left + Vector2.down) * -directionX, 2, collisionMask);

        if (hit || collisions.descendingSlope) {
            return true;
        } else {
            return false;
        }
    }
    private void VerticalCollisions(ref Vector2 moveAmount) {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + SKIN_WIDTH;

        for (int i = 0; i < verticalRayCount; i++) {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit) {
                if (hit.collider.tag == "Through") {
                    if (directionY == 1 || hit.distance == 0) {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform) {
                        continue;
                    }
                    if (input.y == -1) {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope) {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
                collisions.collisionObject = hit.collider.gameObject;

            }
        }

        if (collisions.climbingSlope) {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + SKIN_WIDTH;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle) {
                    moveAmount.x = (hit.distance - SKIN_WIDTH) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal) {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY) {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    private void DescendSlope(ref Vector2 moveAmount) {

        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + SKIN_WIDTH, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + SKIN_WIDTH, collisionMask);
        if (maxSlopeHitLeft ^ maxSlopeHitRight) {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }

        if (!collisions.slidingDownMaxSlope) {
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
                    if (Mathf.Sign(hit.normal.x) == directionX) {
                        if (hit.distance - SKIN_WIDTH <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)) {
                            float moveDistance = Mathf.Abs(moveAmount.x);
                            float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendmoveAmountY;

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    private void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount) {

        if (hit) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle > maxSlopeAngle) {
                moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }

    }

    private void ResetFallingThroughPlatform() {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public GameObject collisionObject;

        public void Reset() {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;
            collisionObject = null;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}