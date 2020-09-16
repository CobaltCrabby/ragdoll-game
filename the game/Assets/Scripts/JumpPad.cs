using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    private ThirdPersonMovement TPM;

    void Start() {
        TPM = FindObjectOfType<ThirdPersonMovement>();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 9) {
            foreach (Rigidbody rb in TPM.rigidbodies) {
                rb.velocity = Vector3.zero;
            }
            TPM.gameObject.GetComponent<Rigidbody>().velocity = Vector3.up * 90;
        }
    }

}
