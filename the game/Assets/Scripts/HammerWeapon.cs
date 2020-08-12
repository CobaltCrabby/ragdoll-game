using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerWeapon : MonoBehaviour
{
    public Vector3 grapplePoint;
    public LayerMask whatIsGrapple, enemyLayer;
    public Transform gunTip, cam, player;
    public float hammerForce, enemyGrappleForce, maxForce;
    public bool isEnemyGrappling, isRegularGrappling;
    public Rigidbody armRB;
    public Rigidbody[] RB;
    public ParticleSystem particles;
    private LineRenderer lr;
    private float maxDistance = 25;
    private SpringJoint joint;

    void Awake() {
        lr = GetComponent<LineRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        armRB = player.gameObject.GetComponent<Rigidbody>();
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
            addGravity();
        }

        if (Input.GetKeyDown("q")) {
            SwingHammer();
        }

        else if (Input.GetKeyUp("q")) {
            particles.Stop();
        }

        if (isEnemyGrappling) {
            Vector3 direction = Vector3.Normalize(grapplePoint - armRB.position);
            if (armRB.velocity.magnitude <= maxForce) {
                armRB.AddForce(direction * enemyGrappleForce);
            }
        }
    }

    void LateUpdate() {
        DrawRope();
    }

    void StartGrapple() {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitEnemy, maxDistance, enemyLayer)) {
            grapplePoint = hitEnemy.point;
            isEnemyGrappling = true;
            lr.positionCount = 2;
            removeGravity();
        }

        else if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, whatIsGrapple)) {
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

            isRegularGrappling = true;
        }
    }

    void DrawRope() {
        if (!isEnemyGrappling && !isRegularGrappling) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    public void StopGrapple() {
        transform.localEulerAngles = new Vector3(180f, 0f, 0f);
        lr.positionCount = 0;
        isEnemyGrappling = false;
        isRegularGrappling = false;
        if (joint) {
            Destroy(joint);
        }
    }

    void SwingHammer() {
        particles.Play();
        armRB.AddForce(cam.forward * hammerForce);
    }

    void removeGravity() {
        foreach (Rigidbody rb in RB) {
            rb.useGravity = false;
        }
    }

    void addGravity() {
        foreach (Rigidbody rb in RB) {
            rb.useGravity = true;
        }
    }
}
