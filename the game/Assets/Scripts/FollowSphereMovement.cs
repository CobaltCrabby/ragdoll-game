using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSphereMovement : MonoBehaviour
{
    public GameObject hips, camFollow;
    public Rigidbody rb;
    private Camera cam;
    private float timer;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }

    void Update() {
        timer++;

        hips.transform.position = transform.position;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0f, vertical);
        direction = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up) * direction;
        
        if (rb.velocity.magnitude <= 5) {
            rb.velocity += direction / 1.5f;
        }
    }

    void FixedUpdate() {
        camFollow.transform.position = transform.position + new Vector3(0, 0.5f, 0);
    }

    public void jump() {
        rb.velocity = (Vector3.up * 8) + direction;
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.layer == 8 && timer >= 15) {
            hips.GetComponent<ThirdPersonMovement>().enabled = true;
            timer = 0;
            GetComponent<FollowSphereMovement>().enabled = false;
        }
    }
}
