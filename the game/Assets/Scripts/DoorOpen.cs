using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();   
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 9) {
            if (collision.gameObject.transform.position.x > transform.position.x) {
                print("more");
                animator.SetTrigger("Open Front");
            }

            else if (collision.gameObject.transform.position.x < transform.position.x) {
                print("less");
                animator.SetTrigger("Open Back");
            }
        }
    }
}
