using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectGrappleObject : MonoBehaviour
{
    private HammerWeapon hammer;

    private void Start() {
        hammer = FindObjectOfType<HammerWeapon>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 9) {
            hammer.isDirectGrappling = false;
            hammer.AddGravity();
            hammer.ResetVelocity();
        }
    }
}
