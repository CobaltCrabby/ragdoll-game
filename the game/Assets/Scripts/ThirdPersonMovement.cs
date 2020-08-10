using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour {

    public float speed, maxSpeed, raycastDistance, jumpForce;
    public Transform cameraFollow;
    private Animator blocksAnim;
    private Rigidbody RB;
    private float jumpCooldown;
    private LayerMask groundLayer;
    private Camera cam;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        groundLayer = 1 << 8;
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
        if (RB.velocity.magnitude <= maxSpeed) {
            RB.velocity += direction.normalized * speed;
        }
    }

    void Update() {
        //Falling Blocks raycasting
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit) && hit.transform.gameObject.tag == "Falling Blocks") {
            blocksAnim = hit.transform.gameObject.GetComponent<Animator>();
            blocksAnim.SetTrigger("Fall");
        }

        //Jumping raycasting
        jumpCooldown++;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded && jumpCooldown >= 15) {
            RB.velocity += Vector3.up * jumpForce;
            jumpCooldown = 0;
        }

        cameraFollow.position = transform.position + new Vector3(0, 0.5f, 0);
    }
}
