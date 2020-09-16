using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeGate : MonoBehaviour
{
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Open");
    }
}
