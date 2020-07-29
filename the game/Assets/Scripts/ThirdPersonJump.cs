using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonJump : MonoBehaviour {
    public float raycastDistance, jumpHeight;
    public LayerMask groundLayer, jumpLayer;
    public bool isGrounded, isJumpPadReady, isJumpPad;
    public Rigidbody RB;
    public Transform jumpPad;
    public float jumpCooldown = 0f;

    void Update() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
        isJumpPadReady = Physics.Raycast(transform.position, Vector3.down, 1f, jumpLayer);
        float height = Mathf.Sqrt(-2f * jumpHeight * Physics.gravity.y);
        jumpCooldown++;
        if (Input.GetButtonDown("Jump") && isGrounded) {
            RB.AddForce(new Vector3(0f, height * 1000, 0f));
        }
        if (isJumpPadReady && jumpCooldown >= 15) {
            RB.AddForce(new Vector3(0f, height * 1300, 0f));
            isJumpPad = true;
            jumpCooldown = 0;
        }
        else {
            isJumpPad = false;
        }
    }
}
