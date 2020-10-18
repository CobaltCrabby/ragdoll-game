using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour {

    public float speed, maxSpeed, raycastDistance, jumpForce;
    public Transform cameraFollow;
    public Vector3 offset;
    public Rigidbody[] rigidbodies;
    public bool ifHammer;

    private FollowSphereMovement followSphereMovement;
    private HammerWeapon hammerWeapon;
    private Animator blocksAnim;
    private Rigidbody RB;
    private float jumpCooldown;
    private LayerMask groundLayer;
    private Camera cam;
    private Vector3 velocity, zeroVelocity;
    private bool isJump;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        groundLayer = 1 << 8;
        followSphereMovement = FindObjectOfType<FollowSphereMovement>();
        hammerWeapon = FindObjectOfType<HammerWeapon>();
        RB = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }

    void FixedUpdate() {
        //Get input values
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //create the direction vector and rotate based on camera
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up) * direction;

        //apply force if less than maxspeed
        if (RB.velocity.magnitude <= maxSpeed && !hammerWeapon.isEnemyGrappling && !hammerWeapon.isDirectGrappling && !isJump) {
            RB.velocity += direction.normalized * speed;
        }
    }

    void Update() {
        followSphereMovement.transform.position = transform.position;

        //Falling Blocks raycasting
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit) && hit.transform.gameObject.tag == "Falling Blocks") {
            blocksAnim = hit.transform.gameObject.GetComponent<Animator>();
            blocksAnim.SetTrigger("Fall");
        }

        //Jumping raycasting
        jumpCooldown++;
        if (Input.GetButtonDown("Jump") && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit2, raycastDistance, groundLayer) && jumpCooldown >= 15) {
            
            followSphereMovement.enabled = true;
            followSphereMovement.jump();
            GetComponent<ThirdPersonMovement>().enabled = false;
            
            /*if (hit2.transform.gameObject.CompareTag("Ramp")) {
                RB.velocity += Vector3.up * jumpForce * 2;
            }

            else if (hammerWeapon.isDirectGrappling) {
                RB.velocity += Vector3.up * 2;
            }

            else {
                RB.velocity += Vector3.up * jumpForce;
            }*/
        }
    }

    //update camera position
    void LateUpdate() {
        cameraFollow.position = transform.position + new Vector3(0, 0.5f, 0);
    }

    //coroutine for adding camerashake using an offset vector
    public IEnumerator CameraShake(float duration, float magnitude) {

        float elapsed = 0f;

        while (elapsed < duration) {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;

            cameraFollow.position += new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }
        offset = Vector3.zero;
    }

    public IEnumerator TurnOffJump() {
        yield return new WaitForSeconds(0.4f);
        isJump = false;
    }
}
