using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HammerWeapon : MonoBehaviour
{
    public Vector3 grapplePoint;
    public LayerMask whatIsGrapple, enemyLayer, whatIsDirectGrapple;
    public Transform gunTip, cam, player, rightHand;
    public float hammerForce, enemyGrappleForce, maxForce, retractingSpeed;
    public bool isEnemyGrappling, isRegularGrappling, isRetracting, isDirectGrappling, isThrowing, hitThrow;
    public Rigidbody armRB;
    public Rigidbody[] RB;
    public ParticleSystem particles;

    public Material hammerGreen, gemGreen, hammerBlue, gemBlue, hammerRed, gemRed;
    public GameObject gem, hammerMain;
    public Collider hammerHandle, hammerCube;


    private ThirdPersonMovement TPM;
    private LineRenderer lineRenderer;
    private bool colorChange, throwingForward;
    private float maxDistance = 25;
    private float hammerColorTime, throwTime;
    private Material currentHammer, nextHammer, currentGem, nextGem;
    private SpringJoint joint;
    private Vector3 throwDirection, throwPosition, hammerVelocityRef;

    void Awake() {
        armRB = player.gameObject.GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        TPM = FindObjectOfType<ThirdPersonMovement>();
    }

    void Start() {
        particles.Stop();
        currentGem = gemGreen;
        currentHammer = hammerGreen;
    }

    //Detect player input
    void Update() {
        
        //Mouse Button Ifs
        if (Input.GetMouseButtonDown(1)) {
            StartGrapple();
        }

        else if (Input.GetMouseButtonUp(1)) {
            StopGrapple();
            AddGravity();
            isRetracting = true;
        }

        if (Input.GetKeyDown("q")) {
            if (currentHammer == hammerGreen) {
                SwingHammer();
            }

            else if (currentHammer == hammerBlue) {
                ThrowHammer();
            }
        }

        else if (Input.GetKeyUp("q")) {
            particles.Stop();
        }

        //Grappling Ifs
        if (isEnemyGrappling) {
            Vector3 direction = Vector3.Normalize(grapplePoint - armRB.position);
            if (armRB.velocity.magnitude <= maxForce) {
                armRB.AddForce(direction * enemyGrappleForce);
            }
        }

        if (isDirectGrappling) {
            Vector3 direction = Vector3.Normalize(grapplePoint - armRB.position);
            if (armRB.velocity.magnitude <= maxForce) { 
                armRB.AddForce(direction * enemyGrappleForce);
            }

            if (Vector3.Distance(armRB.position, grapplePoint) <= 1f) {
                isDirectGrappling = false;
                ResetVelocity();
                AddGravity();
            }
        }

        //Color Change Ifs
        if (Input.GetKeyDown("f") && !isDirectGrappling && !isEnemyGrappling && !isRegularGrappling) {
            if (currentHammer == hammerGreen) {
                nextGem = gemBlue;
                nextHammer = hammerBlue;
                colorChange = true;
            }

            else if (currentHammer == hammerRed) {
                nextGem = gemGreen;
                nextHammer = hammerGreen;
                colorChange = true;
            }

            else if (currentHammer == hammerBlue) {
                nextGem = gemRed;
                nextHammer = hammerRed;
                colorChange = true;  
            }
        }

        if (colorChange) {
            
            hammerMain.GetComponent<MeshRenderer>().materials[2].Lerp(currentHammer, nextHammer, hammerColorTime);
            gem.GetComponent<MeshRenderer>().material.Lerp(currentGem, nextGem, hammerColorTime);
            lineRenderer.material.Lerp(currentHammer, nextHammer, hammerColorTime);

            hammerColorTime += Time.deltaTime * 3;
            if (hammerColorTime >= 1) {
                hammerColorTime = 0;
                colorChange = false;
                currentHammer = nextHammer;
                currentGem = nextGem;
            }
        }

        //Throwing Ifs
        if (isThrowing) {
            if (throwingForward) {
                transform.position = Vector3.Lerp(throwPosition, throwPosition + throwDirection * 20, throwTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(throwDirection) * Quaternion.Euler(180, 90, 270), throwTime * 5);
            }

            else if (!throwingForward && throwTime < 1) {
                transform.position = Vector3.Lerp(throwPosition, rightHand.position, throwTime);
            }

            if (throwTime >= 1) {
                if (throwingForward) {

                    throwingForward = false;
                    throwPosition = transform.position;
                    throwTime = 0f;
                    
                    hammerHandle.enabled = false;
                    hammerCube.enabled = false;
                }

                else if (!throwingForward || hitThrow) {

                    transform.parent = armRB.transform.GetChild(0);
                    throwTime = 1f;
                    
                    hammerHandle.enabled = true;
                    hammerCube.enabled = true;
                    
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(-0.00013f, 0.00696f, -0.0001f), ref hammerVelocityRef, 0.05f);
                    //transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, rightHand.eulerAngles + new Vector3(180, 0, 0), ref hammerVelocityRef, 0.05f);
                    transform.rotation = armRB.rotation * Quaternion.Euler(180, 0, 0);

                    isThrowing = true;
                    hitThrow = false;
                }    
            }
            throwTime += Time.deltaTime;
        }
    }

    //grappling raycasts
    void StartGrapple() {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitEnemy, maxDistance, enemyLayer) && !hitEnemy.transform.gameObject.GetComponentInParent<Enemy1Script>().isDead && currentHammer == hammerRed) {
            grapplePoint = hitEnemy.point;
            isEnemyGrappling = true;
            RemoveGravity();
        }

        else if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit2, maxDistance) && currentHammer == hammerBlue && hit2.transform.CompareTag("Direct Grapple")) {
            grapplePoint = hit2.point;
            isDirectGrappling = true;
            RemoveGravity();
        }

        else if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxDistance, whatIsGrapple) && currentHammer == hammerGreen) {
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

            isRegularGrappling = true;
        }
    }

    public void StopGrapple() {
        transform.localEulerAngles = new Vector3(180f, 0f, 0f);
        isEnemyGrappling = false;
        isRegularGrappling = false;
        isDirectGrappling = false;
        if (joint) {
            Destroy(joint);
        }
    }

    void SwingHammer() {
        Transform enemyPosition = GetClosestEnemy();
        particles.Play();
        if (enemyPosition != null && Vector3.Distance(enemyPosition.position, transform.position) <= 2f) {
            armRB.AddForce((enemyPosition.position - transform.position).normalized * hammerForce);
        }

        else {
            armRB.AddForce(cam.forward * hammerForce);
        }
    }

    void ThrowHammer() {
        transform.parent = null;
        throwingForward = true;
        throwDirection = Vector3.Normalize((cam.position + cam.forward * 20) - transform.position);
        throwPosition = transform.position;
        isThrowing = true;
        throwTime = 0;
        armRB.AddForce(throwDirection * hammerForce);
    }

    void RemoveGravity() {
        foreach (Rigidbody rb in RB) {
            rb.useGravity = false;
        }
    }

    public void AddGravity() {
        foreach (Rigidbody rb in RB) {
            rb.useGravity = true;
        }
    }

    public void ResetVelocity() {
        foreach (Rigidbody rb in TPM.rigidbodies) {
            rb.velocity = Vector3.zero;
        }
    }

    Transform GetClosestEnemy() {
        List<Enemy1Script> enemiesObject = FindObjectsOfType<Enemy1Script>().ToList<Enemy1Script>();
        List<Transform> enemies = new List<Transform>();

        foreach (Enemy1Script item in enemiesObject) {
            enemies.Add(item.transform);
        }

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in enemies) {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && !potentialTarget.gameObject.GetComponentInChildren<Enemy1Script>().isDead) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}