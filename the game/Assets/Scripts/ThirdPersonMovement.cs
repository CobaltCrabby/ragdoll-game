using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody RB;
    public Transform cam;
    public float speed = 3f;
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Quaternion.AngleAxis(cam.rotation.eulerAngles.y, Vector3.up) * direction;
        Vector3 moveVector = direction * speed;

        RB.velocity += moveVector;
    }
}
