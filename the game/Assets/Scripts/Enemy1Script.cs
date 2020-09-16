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
    public Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        animator.enabled = true;
        foreach (Rigidbody rb in rigidbodys) {
            rb.isKinematic = true;
            rb.gameObject.AddComponent<EnemyCollider>();
        }
    }
}
