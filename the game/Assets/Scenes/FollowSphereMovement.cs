using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSphereMovement : MonoBehaviour
{
    public GameObject hips, camFollow;
    private Rigidbody rb;
    private Camera cam;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }

    void Update() {
        
        hips.transform.position = transform.position;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        direction = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up) * direction;
        
        if (rb.velocity.magnitude <= 5) {
            rb.velocity += direction / 4;
        }

        if (Input.GetButtonDown("Jump")){
            rb.velocity += Vector3.up * 7f;
        }
    }

    void FixedUpdate() {
        camFollow.transform.position = transform.position + new Vector3(0, 0.5f, 0);
    }
}
