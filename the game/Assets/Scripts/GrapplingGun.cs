using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    public Vector3 grapplePoint;
    public LayerMask whatIsGrapple;
    public Transform gunTip, cam, player;
    public ThirdPersonCamera TPC;
    private float maxDistance = 50f;
    private SpringJoint joint;
    public bool isGrappling = false;
    
    void Awake() {
        lr = GetComponent<LineRenderer>();    
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    void LateUpdate() {
        DrawRope();
    }
    void StartGrapple() {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, whatIsGrapple)) {
            isGrappling = true;
            TPC.playerDisappear = true;
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.17f;
            joint.minDistance = distanceFromPoint * 0.07f;

            joint.spring = 50f;
            joint.damper = 27f;
            joint.massScale = 1.2f;
            joint.anchor += new Vector3(0f, 0.0005f, 0f);

            lr.positionCount = 2;
        }
    }

    void DrawRope() {
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple() {
        transform.localEulerAngles = new Vector3(180f, -20f, -90f);
        lr.positionCount = 0;
        isGrappling = false;
        Destroy(joint);
    }
}
