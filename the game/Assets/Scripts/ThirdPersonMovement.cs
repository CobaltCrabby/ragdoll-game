using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour {

    public float speed, maxSpeed, raycastDistance, jumpForce;
    public Transform cameraFollow;
    public Vector3 offset;
    private HammerWeapon hammerWeapon;
    private Animator blocksAnim;
    private Rigidbody RB;
    private float jumpCooldown;
    private LayerMask groundLayer;
    private Camera cam;
    private Vector3 velocity;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        groundLayer = 1 << 8;
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
        if (RB.velocity.magnitude <= maxSpeed && !hammerWeapon.isEnemyGrappling) {
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
    }

    //update camera position
    void LateUpdate() {
        cameraFollow.position = transform.position + new Vector3(0, 0.5f, 0) + offset;
    }

    public IEnumerator cameraShake(float duration, float magnitude) {

        float elapsed = 0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            offset = Vector3.SmoothDamp(offset, new Vector3(x, y, 0f), ref velocity, 0.1f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        offset = Vector3.SmoothDamp(offset, Vector3.zero, ref velocity, 0.1f);
    }
}
