using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerWeapon : MonoBehaviour
{
    public Vector3 grapplePoint;
    public LayerMask whatIsGrapple;
    public Transform gunTip, cam, player;
    public float hammerForce;
    public bool isGrappling = false;
    public Rigidbody armRB;
    public ParticleSystem particles;
    private LineRenderer lr;
    private float maxDistance = 50;
    private SpringJoint joint;
    private Rigidbody RB;
    
    void Awake() {
        lr = GetComponent<LineRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        armRB = player.gameObject.GetComponent<Rigidbody>();
        RB = GetComponent<Rigidbody>();
    }

    void Start() {
        particles.Stop();
    }


    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(1)) {
            StopGrapple();
        }

        if (Input.GetKeyDown("q")) {
            SwingHammer();
        }

        else if (Input.GetKeyUp("q")) {
            particles.Stop();
        }
    }

    void LateUpdate() {
        DrawRope();
    }

    void StartGrapple() {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, whatIsGrapple)) {
            isGrappling = true;
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
            joint.anchor += new Vector3(0f, 0.005f, 0f);

            lr.positionCount = 2;
        }
    }

    void DrawRope() {
        if (!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple() {
        transform.localEulerAngles = new Vector3(180f, 0f, 0f);
        lr.positionCount = 0;
        isGrappling = false;
        Destroy(joint);
    }

    void SwingHammer() {
        particles.Play();
        armRB.AddForce(cam.forward * hammerForce);
    }
}
