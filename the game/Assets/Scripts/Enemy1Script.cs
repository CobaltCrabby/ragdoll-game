using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Script : MonoBehaviour
{
    public bool isDead, didSee;
    public Rigidbody[] rigidbodys;
    public HammerWeapon hammer;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        animator.enabled = true;
        foreach (Rigidbody rb in rigidbodys) {
            rb.isKinematic = true;
        }
    }

    void Update() {
        if (isDead) {
            animator.enabled = false;
            foreach (Rigidbody rb in rigidbodys) {
                rb.isKinematic = false;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Hammer" && hammer.particles.isPlaying && hammer.armRB.velocity.magnitude >= 2) {
            isDead = true;
        }    
    }
}
