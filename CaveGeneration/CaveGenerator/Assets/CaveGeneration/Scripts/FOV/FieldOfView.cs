using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public float maskCutawayDst = .1f;

    [HideInInspector]
    public float focusedAngle = 0;

    private MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start() {
        viewMesh = new Mesh();
        viewMeshFilter = GetComponentInChildren<MeshFilter>();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    private IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void LateUpdate() {
        DrawFieldOfView();
    }

    private void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(transform.right, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector2.Distance(transform.position, target.position);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask);

                if (!hit) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public void Focus() {
        //TODO: Decrease the view angle to only the focused object.
    }

    private void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
           // Debug.DrawLine(transform.position, newViewCast.point, Color.black);

            if (i > 0) {

                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector2.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector2.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.right * maskCutawayDst;

            /*if (i < vertexCount - 2) {
                triangles[i * 3] = 0    ;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }*/
            if (i < vertexCount - 2) {
                triangles[i * 3] = i + 2;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 0;
            }

        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, false);
        //RaycastHit hit;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);


        if (hit) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector2 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dst, float _angle) {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 _pointA, Vector2 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}